using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JASON_Compiler
{
    public class Node
    {
        public List<Node> Children = new List<Node>();

        public string Name;
        public Node(string N)
        {
            this.Name = N;
        }
    }
    public class Parser
    {
        int InputPointer = 0;
        List<Token> TokenStream;
        public Node root;

        public Node StartParsing(List<Token> TokenStream)
        {
            this.InputPointer = 0;
            this.TokenStream = TokenStream;
            root = new Node("Program");
            root.Children.Add(Tiny_Program());
            return root;
        }

        /*
        Node Program()
        {
            Node program = new Node("Program");
            program.Children.Add(Header());
            program.Children.Add(DeclSec());
            program.Children.Add(Block());
            program.Children.Add(match(Token_Class.Dot));
            MessageBox.Show("Success");
            return program;
        }
        
        Node Header()
        {
            Node header = new Node("Header");
            // write your code here to check the header sructure
            return header;
        }
        Node DeclSec()
        {
            Node declsec = new Node("DeclSec");
            // write your code here to check atleast the declare sturcure 
            // without adding procedures
            return declsec;
        }
        Node Block()
        {
            Node block = new Node("block");
            // write your code here to match statements
            return block;                                               
        }
        */
        // Implement your logic here

        Node Tiny_Program()
        {
            Node Tiny_Program = new Node("Tiny_Program");

            


                Tiny_Program.Children.Add(Fn_list());
                Tiny_Program.Children.Add(Main_Function());



            
                


            return Tiny_Program;

        }
        Node Declaration_Statement()
        {
            Node declarationStmt = new Node("Declaration_Statement");
            declarationStmt.Children.Add(Datatype());
            declarationStmt.Children.Add(DeclarationList());
            declarationStmt.Children.Add(match(Token_Class.Semicolon));
            return declarationStmt;
        }

        Node DeclarationList()
        {
            Node declarationList = new Node("DeclarationList");
            declarationList.Children.Add(Declaration());
            while (InputPointer < TokenStream.Count &&
                   TokenStream[InputPointer].token_type == Token_Class.Comma)
            {
                declarationList.Children.Add(match(Token_Class.Comma));
                declarationList.Children.Add(Declaration());
            }
            return declarationList;
        }
        // int x, y, z, h;
        Node Declaration()
        {
            Node declaration = new Node("Declaration");
            declaration.Children.Add(match(Token_Class.Identifier));
            if (InputPointer < TokenStream.Count &&
                TokenStream[InputPointer].token_type == Token_Class.AssignmentStatement)
            {
                declaration.Children.Add(match(Token_Class.AssignmentStatement));
                declaration.Children.Add(Expression());
            }
            return declaration;
        }// program->fn_list main
         // fn_list->fn_statement fn_list | e



        //fp->datatpe id fpdash
        //fpdash->,data id fpdash | e

        Node Parameters_Call()
        {
            Node pc = new Node("Parameters_Call");








            if (TokenStream[InputPointer].token_type == Token_Class.RBracket)
            {
                return pc;
            }
                pc.Children.Add(Term());

                while (InputPointer < TokenStream.Count && TokenStream[InputPointer].token_type == Token_Class.Comma)
                {
                    pc.Children.Add(match(Token_Class.Comma));
                    pc.Children.Add(Term());
                }






            return pc;
        }



        Node Parameters()      //int x;

        {

            Node p = new Node("Parameters");




            if (TokenStream[InputPointer].token_type == Token_Class.RBracket)
            {
                return p;
            }
                p.Children.Add(Datatype());



                p.Children.Add(match(Token_Class.Identifier));

                p.Children.Add(MoreParameters());




            


            return p;

        }



        Node MoreParameters()

        {

            Node more_p = new Node("MoreParameters");


            while (InputPointer < TokenStream.Count)
            {
                if (TokenStream[InputPointer].token_type == Token_Class.Comma)
                {
                    more_p.Children.Add(match(Token_Class.Comma));

                    more_p.Children.Add(Datatype());

                    more_p.Children.Add(match(Token_Class.Identifier));

                }
                else
                {
                    break;
                }


            }

            return more_p;

        }


        Node Function_Body()
        {
            Node Function_Body = new Node("Function_Body");

            


                Function_Body.Children.Add(match(Token_Class.LCurlyBracket));



                Function_Body.Children.Add(Statements());//statements -> State Statements | ɛ




                Function_Body.Children.Add(Return_Statement());




                Function_Body.Children.Add(match(Token_Class.RCurlyBracket));

            


            return Function_Body;

        }
        Node Function_Declaration()
        {
            Node Function_Declaration = new Node("Function_Declaration");

            


                Function_Declaration.Children.Add(Datatype());
                Function_Declaration.Children.Add(Function_Name());

                Function_Declaration.Children.Add(match(Token_Class.LBracket));


                Function_Declaration.Children.Add(Parameters());//statements -> State Statements | ɛ


                Function_Declaration.Children.Add(match(Token_Class.RBracket));

            


            return Function_Declaration;

        }
        Node Function_Statement()
        {
            Node Function_Statement = new Node("Function_Statement");

            


                Function_Statement.Children.Add(Function_Declaration());
                Function_Statement.Children.Add(Function_Body());

            


            return Function_Statement;

        }
        Node Main_Function()
        {
            Node Main_Function = new Node("Main_Function");

            


                Main_Function.Children.Add(Datatype());


                Main_Function.Children.Add(match(Token_Class.Main));

                Main_Function.Children.Add(match(Token_Class.LBracket));


                Main_Function.Children.Add(match(Token_Class.RBracket));

                Main_Function.Children.Add(Function_Body());

            


            return Main_Function;

        }

        Node If_Statement()
        {
            Node ifStmt = new Node("IfStatement");
            ifStmt.Children.Add(match(Token_Class.If));
            ifStmt.Children.Add(Condition());
            ifStmt.Children.Add(match(Token_Class.Then));
            ifStmt.Children.Add(Statements());
            ifStmt.Children.Add(IfTail());
            ifStmt.Children.Add(match(Token_Class.End));
            return ifStmt;
        }

        Node IfTail()
        {
            Node tail = new Node("IfTail");
            Token current = TokenStream[InputPointer];

            if (current.token_type == Token_Class.ElseIf)
            {
                tail.Children.Add(ElseIf_Statement());
                tail.Children.Add(IfTail());
            }
            else if (current.token_type == Token_Class.Else)
            {
                tail.Children.Add(Else_Statement());
            }



            return tail;
        }

        Node ElseIf_Statement()
        {
            Node elseif = new Node("ElseIfStatement");
            elseif.Children.Add(match(Token_Class.ElseIf));
            elseif.Children.Add(Condition());
            elseif.Children.Add(match(Token_Class.Then));
            elseif.Children.Add(Statements());
            return elseif;
        }


        Node Else_Statement()
        {
            Node elseStmt = new Node("ElseStatement");
            elseStmt.Children.Add(match(Token_Class.Else));
            elseStmt.Children.Add(Statements());
            return elseStmt;
        }

        Node Write_Statement()
        {
            Node writeStmt = new Node("Write_Statement");
            
                writeStmt.Children.Add(match(Token_Class.Write));

                if (TokenStream[InputPointer].token_type == Token_Class.Endl)
                {
                    writeStmt.Children.Add(match(Token_Class.Endl));
                }
                else
                {
                    writeStmt.Children.Add(Expression());
                }
                writeStmt.Children.Add(match(Token_Class.Semicolon));
            


            return writeStmt;
        }

        Node Read_Statement()
        {
            Node read = new Node("ReadStatement");
            read.Children.Add(match(Token_Class.Read));
            read.Children.Add(match(Token_Class.Identifier));
            read.Children.Add(match(Token_Class.Semicolon));
            return read;
        }

        Node Repeat_Statement()
        {
            Node repeat = new Node("RepeatStatement");
            repeat.Children.Add(match(Token_Class.Repeat));
            repeat.Children.Add(Statements());
            repeat.Children.Add(match(Token_Class.Until));
            repeat.Children.Add(Condition());
            //repeat.Children.Add(match(Token_Class.Semicolon));
            return repeat;
        }

        Node Return_Statement()
        {
            Node ret = new Node("ReturnStatement");
            ret.Children.Add(match(Token_Class.RETURN));
            ret.Children.Add(Expression());
            ret.Children.Add(match(Token_Class.Semicolon));
            return ret;
        }

        Node Condition()
        {
            Node condition = new Node("Condition");
            condition.Children.Add(match(Token_Class.Identifier));
            condition.Children.Add(ConditionOp());
            condition.Children.Add(Term());
            condition.Children.Add(MoreConditions());
            return condition;
        }

        Node MoreConditions()
        {
            Node more = new Node("MoreConditions");
            Token current = TokenStream[InputPointer];

            if (current.token_type == Token_Class.AndOp)
            {
                more.Children.Add(match(Token_Class.AndOp));
                more.Children.Add(match(Token_Class.Identifier));
                more.Children.Add(ConditionOp());
                more.Children.Add(Term());
                more.Children.Add(MoreConditions());
            }
            else if (current.token_type == Token_Class.OrOp)
            {
                more.Children.Add(match(Token_Class.OrOp));
                more.Children.Add(match(Token_Class.Identifier));
                more.Children.Add(ConditionOp());
                more.Children.Add(Term());
                more.Children.Add(MoreConditions());
            }

            return more;
        }

        Node ConditionOp()
        {
            Node op = new Node("ConditionOp");
            Token current = TokenStream[InputPointer];

            if (current.token_type == Token_Class.EqualOp)
                op.Children.Add(match(Token_Class.EqualOp));
            else if (current.token_type == Token_Class.LessThanOp)
                op.Children.Add(match(Token_Class.LessThanOp));
            else if (current.token_type == Token_Class.GreaterThanOp)
                op.Children.Add(match(Token_Class.GreaterThanOp));
            else
                op.Children.Add(match(Token_Class.NotEqualOp));


            return op;
        }






        Node Statements()
        {
            Node statements = new Node("Statements");

            while (InputPointer < TokenStream.Count &&
                  (TokenStream[InputPointer].token_type == Token_Class.Identifier ||
                   TokenStream[InputPointer].token_type == Token_Class.Int ||
                   TokenStream[InputPointer].token_type == Token_Class.Float ||
                   TokenStream[InputPointer].token_type == Token_Class.String ||
                   TokenStream[InputPointer].token_type == Token_Class.Write ||
                   TokenStream[InputPointer].token_type == Token_Class.Read ||
                   TokenStream[InputPointer].token_type == Token_Class.If ||
                   TokenStream[InputPointer].token_type == Token_Class.Repeat
                   ))
            {
                statements.Children.Add(Statement());
            }

            return statements;
        }

        Node Statement()
        {
            Node statement = new Node("Statement");

            
                Token_Class current = TokenStream[InputPointer].token_type;

                if (current == Token_Class.Identifier)
                {
                    if (InputPointer + 1 < TokenStream.Count && TokenStream[InputPointer + 1].token_type == Token_Class.LBracket)
                    {
                        statement.Children.Add(Function_Call());
                        statement.Children.Add(match(Token_Class.Semicolon));
                    }
                    else
                    {
                        statement.Children.Add(Assignment_Statement());
                    }
                }
                else if (current == Token_Class.Int || current == Token_Class.Float || current == Token_Class.String)
                    statement.Children.Add(Declaration_Statement());

                else if (current == Token_Class.Write)
                    statement.Children.Add(Write_Statement());

                else if (current == Token_Class.Read)
                    statement.Children.Add(Read_Statement());

                else if (current == Token_Class.If)
                    statement.Children.Add(If_Statement());

                else if (current == Token_Class.Repeat)
                    statement.Children.Add(Repeat_Statement());

                


            
            return statement;
        }

        Node Function_Call()
        {
            Node functionCall = new Node("Function_Call");

            
                functionCall.Children.Add(Function_Name());

                functionCall.Children.Add(match(Token_Class.LBracket));

                functionCall.Children.Add(Parameters_Call());

                functionCall.Children.Add(match(Token_Class.RBracket));

                //functionCall.Children.Add(match(Token_Class.Semicolon));
            

            return functionCall;
        }

        Node Assignment_Statement()
        {
            Node assign = new Node("Assignment_Statement");
            assign.Children.Add(match(Token_Class.Identifier));
            assign.Children.Add(match(Token_Class.AssignmentStatement));
            assign.Children.Add(Expression());
            assign.Children.Add(match(Token_Class.Semicolon));
            return assign;
        }

        Node Expression()
        {
            Node expr = new Node("Expression");

            
                if (TokenStream[InputPointer].token_type == Token_Class.String)
                {
                    expr.Children.Add(match(Token_Class.String));
                }
                else if (CheckTermStart())
                {
                    if (InputPointer + 1 < TokenStream.Count)
                    {
                        var t = TokenStream[InputPointer + 1].token_type;
                        bool b = t == Token_Class.PlusOp || t == Token_Class.MinusOp ||
                               t == Token_Class.MultiplyOp || t == Token_Class.DivideOp;
                        //expr.Children.Add(Term()); 3
                        if (b)
                        {
                            expr.Children.Add(Equation());
                            /*
                            Node eq = new Node("Equation");
                            eq.Children.Add(expr.Children[0]);
                            eq.Children.Add(Operator_Equation());
                            return eq;
                            */
                        }
                        else
                        {
                            expr.Children.Add(Term());
                        }
                    }
                    else
                    {
                        expr.Children.Add(Term());
                    }
                }
                else
                {
                    expr.Children.Add(Equation());
                }
            

            return expr;
        }
        Node Term()
        {
            Node term = new Node("Term");
            
                if (TokenStream[InputPointer].token_type == Token_Class.Constant)
                {
                    term.Children.Add(match(Token_Class.Constant));
                }
                else
                {

                    if (InputPointer + 1 < TokenStream.Count && TokenStream[InputPointer].token_type == Token_Class.Identifier &&
                    TokenStream[InputPointer + 1].token_type == Token_Class.LBracket)
                    {
                        term.Children.Add(Function_Call());
                    }
                    else
                    {
                        term.Children.Add(match(Token_Class.Identifier));
                    }
                }
            
            return term;
        }

        Node Arithmetic_Operator()
        {
            Node op = new Node("Arithmetic_Operator");

            
                if (TokenStream[InputPointer].token_type == Token_Class.PlusOp)
                {
                    op.Children.Add(match(Token_Class.PlusOp));
                }
                else if (TokenStream[InputPointer].token_type == Token_Class.MinusOp)
                {
                    op.Children.Add(match(Token_Class.MinusOp));
                }
                else if (TokenStream[InputPointer].token_type == Token_Class.MultiplyOp)
                {
                    op.Children.Add(match(Token_Class.MultiplyOp));
                }
                else
                {
                    op.Children.Add(match(Token_Class.DivideOp));
                }
            

            return op;
        }


        Node Equation() // (3*10)+5-6
        {
            Node eq = new Node("Equation");
            if (TokenStream[InputPointer].token_type == Token_Class.LBracket)
            {
                eq.Children.Add(match(Token_Class.LBracket));
                eq.Children.Add(Term());
                eq.Children.Add(Operator_Equation());
                eq.Children.Add(match(Token_Class.RBracket));
                eq.Children.Add(Operator_Equation());
            }
            else
            {

                eq.Children.Add(Term());
                eq.Children.Add(Operator_Equation());
            }
            return eq;
        }
        bool CheckTermStart()
        {
            return TokenStream[InputPointer].token_type == Token_Class.Identifier ||
                   TokenStream[InputPointer].token_type == Token_Class.Constant;
        }

        bool CheckArithmeticOperator()
        {
            var t = TokenStream[InputPointer].token_type;
            return t == Token_Class.PlusOp || t == Token_Class.MinusOp ||
                   t == Token_Class.MultiplyOp || t == Token_Class.DivideOp;
        }
        Node Operator_Equation()
        {
            Node opEq = new Node("Operator_Equation");

            
                if (CheckArithmeticOperator())
                {
                    opEq.Children.Add(Arithmetic_Operator());
                   
                    opEq.Children.Add(Equation());
                    //opEq.Children.Add(Operator_Equation());
                }
            


            return opEq;
        }


        Node Fn_list()
        {
            Node functions = new Node("Functions");

            while (InputPointer < TokenStream.Count && (TokenStream[InputPointer].token_type == Token_Class.Float || TokenStream[InputPointer].token_type == Token_Class.Int || TokenStream[InputPointer].token_type == Token_Class.String))
            {
                if (InputPointer + 1 < TokenStream.Count)
                {
                    if (TokenStream[InputPointer + 1].token_type == Token_Class.Main)
                    {
                        break;
                    }
                }
                functions.Children.Add(Function_Statement());
            }

            return functions;
        }
        Node Datatype()
        {
            Node datatype = new Node("Datatype");
            
                if (TokenStream[InputPointer].token_type == Token_Class.Int)
                {
                    datatype.Children.Add(match(Token_Class.Int));
                }
                else if (TokenStream[InputPointer].token_type == Token_Class.String)
                {
                    datatype.Children.Add(match(Token_Class.String));

                }
                else
                {
                    datatype.Children.Add(match(Token_Class.Float));
                }
            


            return datatype;
        }




        Node Function_Name()
        {
            Node fn_name = new Node("Function_Name");

            

                fn_name.Children.Add(match(Token_Class.Identifier));

            


            return fn_name;
        }




        public Node match(Token_Class ExpectedToken)
        {

            if (InputPointer < TokenStream.Count)
            {
                if (ExpectedToken == TokenStream[InputPointer].token_type)
                {
                    InputPointer++;
                    Node newNode = new Node(ExpectedToken.ToString());

                    return newNode;

                }

                else
                {
                    Errors.Error_List.Add("Parsing Error: Expected "
                        + ExpectedToken.ToString() + " and " +
                        TokenStream[InputPointer].token_type.ToString() +
                        "  found\r\n");
                    InputPointer++;
                    return null;
                }
            }
            else
            {
                Errors.Error_List.Add("Parsing Error: Expected "
                        + ExpectedToken.ToString() + "\r\n");
                InputPointer++;
                return null;
            }
        }

        public static TreeNode PrintParseTree(Node root)
        {
            TreeNode tree = new TreeNode("Parse Tree");
            TreeNode treeRoot = PrintTree(root);
            if (treeRoot != null)
                tree.Nodes.Add(treeRoot);
            return tree;
        }
        static TreeNode PrintTree(Node root)
        {
            if (root == null || root.Name == null)
                return null;
            TreeNode tree = new TreeNode(root.Name);
            if (root.Children.Count == 0)
                return tree;
            foreach (Node child in root.Children)
            {
                if (child == null)
                    continue;
                tree.Nodes.Add(PrintTree(child));
            }
            return tree;
        }
    }
}
