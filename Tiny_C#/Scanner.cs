using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

public enum Token_Class
{
    Identifier, Constant, String, Int, Float, Main, End, Semicolon, Comma,
    Read, Write, Repeat, Until, If, ElseIf, Else, Then, Return, Endl,
    EqualOp, LessThanOp, AndOp, OrOp, GreaterThanOp, NotEqualOp, PlusOp, MinusOp,
    MultiplyOp, DivideOp, AssignmentStatement, Declaration_Statement, LCurlyBracket,
    RCurlyBracket, RETURN, Condition, SingelQuote, DoubleQuote, LBracket, RBracket,
    Comment

}
namespace JASON_Compiler
{
    public class Token
    {
        public string lex;
        public Token_Class token_type;
    }

    public class Scanner
    {
        public List<Token> Tokens = new List<Token>();
        Dictionary<string, Token_Class> ReservedWords = new Dictionary<string, Token_Class>();
        Dictionary<string, Token_Class> Operators = new Dictionary<string, Token_Class>();

        public Scanner()
        {
            ReservedWords.Add("if", Token_Class.If);
            ReservedWords.Add("end", Token_Class.End);
            ReservedWords.Add("else", Token_Class.Else);
            ReservedWords.Add("elseif", Token_Class.ElseIf);
            ReservedWords.Add("int", Token_Class.Int);
            ReservedWords.Add("read", Token_Class.Read);
            ReservedWords.Add("then", Token_Class.Then);
            ReservedWords.Add("until", Token_Class.Until);
            ReservedWords.Add("write", Token_Class.Write);
            ReservedWords.Add("float", Token_Class.Float);
            ReservedWords.Add("string", Token_Class.String);
            ReservedWords.Add("repeat", Token_Class.Repeat);
            ReservedWords.Add("return", Token_Class.RETURN);
            ReservedWords.Add("endl", Token_Class.Endl);
            ReservedWords.Add("main", Token_Class.Main);



            Operators.Add(";", Token_Class.Semicolon);
            Operators.Add(",", Token_Class.Comma);
            Operators.Add("(", Token_Class.LBracket);
            Operators.Add(")", Token_Class.RBracket);
            Operators.Add("{", Token_Class.LCurlyBracket);
            Operators.Add("}", Token_Class.RCurlyBracket);
            Operators.Add("=", Token_Class.EqualOp);
            Operators.Add("<", Token_Class.LessThanOp);
            Operators.Add(">", Token_Class.GreaterThanOp);
            Operators.Add("<>", Token_Class.NotEqualOp);
            Operators.Add(":=", Token_Class.AssignmentStatement);
            Operators.Add("+", Token_Class.PlusOp);
            Operators.Add("-", Token_Class.MinusOp);
            Operators.Add("/", Token_Class.DivideOp);
            Operators.Add("*", Token_Class.MultiplyOp);
            Operators.Add("&&", Token_Class.AndOp);
            Operators.Add("||", Token_Class.OrOp);
            Operators.Add("'", Token_Class.SingelQuote);
            Operators.Add("\"", Token_Class.DoubleQuote);



        }


        public void StartScanning(string SourceCode)
        {
            for (int i = 0; i < SourceCode.Length; i++)
            {
                int j = i;
                char CurrentChar = SourceCode[i];
                string CurrentLexeme = CurrentChar.ToString();

                if (CurrentChar == ' ' || CurrentChar == '\r' || CurrentChar == '\n' || CurrentChar == '\t')
                    continue;

                if (CurrentChar >= 'A' && CurrentChar <= 'z') //if you read a character
                {
                    j++;
                    if (j < SourceCode.Length)
                    {
                        while ((SourceCode[j] >= 'A' && SourceCode[j] <= 'z') || (SourceCode[j] >= '0' && SourceCode[j] <= '9'))
                        {
                            CurrentLexeme += SourceCode[j].ToString();
                            j++;
                            if (j >= SourceCode.Length)
                            {
                                break;
                            }
                        }
                    }

                    FindTokenClass(CurrentLexeme);

                    i = j - 1;
                }



                else if (CurrentChar >= '0' && CurrentChar <= '9')
                {
                    j++;
                    if (j < SourceCode.Length)
                    {
                        while ((SourceCode[j] >= '0' && SourceCode[j] <= '9') || (SourceCode[j].ToString() == ".") || (SourceCode[j] >= 'A' && SourceCode[j] <= 'z'))
                        {

                            CurrentLexeme += SourceCode[j].ToString();
                            j++;
                            if (j >= SourceCode.Length)
                            {
                                break;
                            }
                        }



                    }
                    FindTokenClass(CurrentLexeme);
                    i = j - 1;
                }

                //"sdasdasd;
                else if (CurrentChar == '\"')
                {
                    j++;
                    if (j < SourceCode.Length)
                    {
                        while ((SourceCode[j] != '\"'))
                        {
                            CurrentLexeme += SourceCode[j].ToString();
                            j++;
                            if (j >= SourceCode.Length)
                            {
                                break;
                            }
                        }
                        if (j < SourceCode.Length)
                        {
                            CurrentLexeme += SourceCode[j];
                        }
                    }
                    FindTokenClass(CurrentLexeme);
                    j++;
                    i = j - 1;

                }
                
                else if (CurrentChar == '/' && SourceCode[j + 1] == '*')
                {
                    CurrentLexeme += SourceCode[j + 1];
                    j += 2;
                    if (j + 1 < SourceCode.Length)
                    {
                        while ((SourceCode[j] != '*') && (SourceCode[j + 1] != '/'))
                        {
                            CurrentLexeme += SourceCode[j].ToString();
                            j++;
                            if (j + 1 >= SourceCode.Length)
                            {
                                break;
                            }
                        }
                        if (j < SourceCode.Length)
                        {
                            CurrentLexeme += SourceCode[j].ToString();
                        }
                        if (j + 1 < SourceCode.Length)
                        {
                            CurrentLexeme += SourceCode[j + 1].ToString();
                        }
                    }
                    j += 2;
                    FindTokenClass(CurrentLexeme);
                    i = j - 1;

                }

                else
                {
                    if (j + 1 < SourceCode.Length)
                    {
                        if ((CurrentChar == '<' && SourceCode[j + 1] == '>') || (CurrentChar == ':' && SourceCode[j + 1] == '=') || (CurrentChar == '&' && SourceCode[j + 1] == '&') || (CurrentChar == '|' && SourceCode[j + 1] == '|'))
                        {
                            j++;
                            CurrentLexeme += SourceCode[j].ToString();


                        }
                    }
                    j++;
                    FindTokenClass(CurrentLexeme);
                    i = j - 1;
                }
            }

            JASON_Compiler.TokenStream = Tokens;
        }
        void FindTokenClass(string Lex)
        {
            Token_Class TC;
            Token Tok = new Token();
            Tok.lex = Lex;

            //Is it a reserved word?
            if (ReservedWords.TryGetValue(Tok.lex, out TC))
            {
                Tok.token_type = TC;
            }


            //Is it an identifier?
            else if (isIdentifier(Lex))
            {
                TC = Token_Class.Identifier;
                Tok.token_type = TC;

            }

            //Is it a Constant?
            else if (isConstant(Lex))
            {
                TC = Token_Class.Constant;
                Tok.token_type = TC;
            }
            else if (isComment(Lex))
            {
                return;
            }
            else if (isString(Lex))
            {
                TC = Token_Class.String;
                Tok.token_type = TC;
            }
            else if (Operators.TryGetValue(Lex, out TC))
            {
                Tok.token_type = TC;
            }
            //Is it an undefined?
            else
            {
                Errors.Error_List.Add(Lex);
                return;
            }
            Tokens.Add(Tok);
        }
        bool isIdentifier(string lex)
        {

            bool isValid = true;
            // Check if the lex is an identifier or not.
            Regex ex = new Regex(@"^[a-zA-Z][a-zA-Z0-9]*$");
            if (!ex.IsMatch(lex))
            {
                isValid = false;

            }


            return isValid;
        }
        bool isString(string lex)
        {

            bool isValid = true;

            Regex ex = new Regex("^(\")(.|\n)*(\")$");
            //no @?????
            if (!ex.IsMatch(lex))
            {
                isValid = false;
            }
            return isValid;
        }

        bool isComment(string lex)
        {
            bool isValid = true;

            Regex ex = new Regex(@"^(/)(\)(.|\n)(\*)(/)$");
            if (!ex.IsMatch(lex))
            {
                isValid = false;
            }

            return isValid;
        }

        bool isConstant(string lex)
        {
            bool isValid = true;
            // Check if the lex is a constant (Number) or not.
            Regex ex = new Regex(@"^[0-9]+(\.[0-9]+)?$");
            if (!ex.IsMatch(lex))
            {
                isValid = false;
            }

            return isValid;
        }

    }
}
