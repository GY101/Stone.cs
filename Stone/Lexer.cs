using System;
using System.Collections.Generic;
using System.Data.Common;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Stone
{
    public class Lexer
    {
        /// <summary>
        /// <code>
        ///  JAVA: \p{Punct}	Punctuation: One of !"#$%&'()*+,-./:;<=>?@[\]^_`{|}~
        ///  C#： \p{P}|\p{S} 对应的不确定  ,哈哈哈
        /// </code>
        /// </summary>
        public const string regexPat = @"\s*((//.*)|([0-9]+)|(""(\\""|\\\\|\\n|[^""])*"")|[A-Z_a-z][A-Z_a-z0-9]*|==|<=|>=|&&|\|\||\p{P}|\p{S})?";
        private Regex _regex = new Regex(regexPat, RegexOptions.Compiled);
        private List<Token> queue = new List<Token>();
        private bool _hasMore;
        private TextReader _reader;
        private int _lineNo;
        public Lexer(TextReader reader)
        {
            _hasMore = true;
            _reader = reader;
            _lineNo = 0;
        }
        public Lexer(string text)
        {
            _hasMore = true;
            _reader = new StringReader(text);
            _lineNo = 0;
        }
        public async Task<Token> Read()
        {
            if (await FillQueue(0))
            {
                var token = queue[0];
                queue.RemoveAt(0);
                return token;
            }

            else
                return Token.EOF;
        }
        public async Task<Token> peek(int i)
        {
            if (await FillQueue(i))
                return queue[i];
            else
                return Token.EOF;
        }

        public async Task<bool> FillQueue(int i)
        {
            while (i >= queue.Count)
            {
                if (_hasMore)
                {
                    await ReadLine();
                }
                else
                {
                    return false;
                }
            }
            return true;
        }

        private async Task ReadLine()
        {
            string line = await _reader.ReadLineAsync();
            _lineNo++;
            if (line == null)
            {
                _hasMore = false;
                return;
            }
            int pos = 0;
            var match = _regex.Match(line);
            while (match != Match.Empty)
            {
                if (match.Index == pos)
                {
                    AddToken(_lineNo, match);
                    pos += match.Length;
                }
                else
                    throw new ParseException("bad token at line " + _lineNo);
                match = match.NextMatch();
            }
            queue.Add(new IdToken(_lineNo, Token.EOL));
        }

        private void AddToken(int lineNo, Match match)
        {
            var group1 = match.Groups[1];
            var value = group1.Value;
            if (group1.Success) // if not a space
            {
                if (!match.Groups[2].Success)
                { // if not a comment
                    Token token;
                    if (match.Groups[3].Success)
                        token = new NumToken(lineNo, int.Parse(value));
                    else if (match.Groups[4].Success)
                        token = new StrToken(lineNo, ToStringLiteral(value));
                    else
                        token = new IdToken(lineNo, value);
                    queue.Add(token);
                }
            }

        }

        private string ToStringLiteral(string s)
        {
            StringBuilder sb = new StringBuilder();
            int len = s.Length - 1;
            for (int i = 1; i < len; i++)
            {
                char c = s[i];
                if (c == '\\' && i + 1 < len)
                {
                    char c2 = s[i + 1];
                    if (c2 == '"' || c2 == '\\')
                        c = s[++i];
                    else if (c2 == 'n')
                    {
                        ++i;
                        c = '\n';
                    }
                }
                sb.Append(c);
            }
            return sb.ToString();
        }

        protected class NumToken : Token
        {
            private int value;

            public NumToken(int line, int v) : base(line)
            {
                value = v;
            }
            public override bool IsNumber => true;
            public override string Text => value.ToString();
            public override int Number => value;
        }

        protected class IdToken : Token
        {
            private String text;
            public IdToken(int line, String id) : base(line)
            {
                text = id;
            }
            public override bool IsIdentifier => true;
            public override string Text => text;
        }

        protected class StrToken : Token
        {
            private string literal;
            public StrToken(int line, String str) : base(line)
            {
                literal = str;
            }
            public override bool IsString => true;
            public override string Text => literal;
        }
    }
}
