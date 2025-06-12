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

            if (InputPointer < TokenStream.Count)
            {


                Tiny_Program.Children.Add(Fn_list());
                Tiny_Program.Children.Add(Main_Function());



            }



            return Tiny_Program;

        }
        Node Declaration_Statement()
        {
            Node declarationStmt = new Node("Declaration_Statement");
            if (InputPointer < TokenStream.Count)
            {
                declarationStmt.Children.Add(Datatype());
                declarationStmt.Children.Add(DeclarationList());
                Node n = match(Token_Class.Semicolon);
                if(n!=null) 
                    declarationStmt.Children.Add(n);
            }
            return declarationStmt;
        }

        Node DeclarationList()
        {
            Node declarationList = new Node("DeclarationList");
            if (InputPointer < TokenStream.Count)
            {
                declarationList.Children.Add(Declaration());
                while (InputPointer < TokenStream.Count &&
                       TokenStream[InputPointer].token_type == Token_Class.Comma)
                {
                    Node n = match(Token_Class.Comma);
                    if (n != null) 
                        declarationList.Children.Add(n);
                    declarationList.Children.Add(Declaration());
                }
            }
            return declarationList;
        }
        // int x, y, z, h;
        Node Declaration()
        {
            Node declaration = new Node("Declaration");
            if (InputPointer < TokenStream.Count)
            {
                Node n = match(Token_Class.Identifier);
                if (n != null)
                    declaration.Children.Add(n);
                if (InputPointer < TokenStream.Count &&
                    TokenStream[InputPointer].token_type == Token_Class.AssignmentStatement)
                {
                    n = match(Token_Class.AssignmentStatement);
                    if (n != null)
                        declaration.Children.Add(n);
                    declaration.Children.Add(Expression());
                }
            }
            return declaration;
        }// program->fn_list main
         // fn_list->fn_statement fn_list | e



        //fp->datatpe id fpdash
        //fpdash->,data id fpdash | e

        Node Parameters_Call()
        {
            Node pc = new Node("Parameters_Call");




            if (InputPointer < TokenStream.Count)
            {



                if (TokenStream[InputPointer].token_type == Token_Class.RBracket)
                {
                    return pc;
                }
                pc.Children.Add(Term());

                while (InputPointer < TokenStream.Count && TokenStream[InputPointer].token_type == Token_Class.Comma)
                {
                    Node n = match(Token_Class.Comma);
                    if (n != null)
                        pc.Children.Add(n);
                    pc.Children.Add(Term());
                }


            }



            return pc;
        }



        Node Parameters()      //int x;

        {

            Node p = new Node("Parameters");


            if (InputPointer < TokenStream.Count)
            {

                if (TokenStream[InputPointer].token_type == Token_Class.RBracket)
                {
                    return p;
                }
                p.Children.Add(Datatype());



                Node n = match(Token_Class.Identifier);
                if (n != null)
                    p.Children.Add(n);

                p.Children.Add(MoreParameters());


            }




            return p;

        }



        Node MoreParameters()

        {

            Node more_p = new Node("MoreParameters");
            
                while (InputPointer < TokenStream.Count)
            {
                if (TokenStream[InputPointer].token_type == Token_Class.Comma)

                {
                    Node n = match(Token_Class.Comma);
                    if (n != null)
                        more_p.Children.Add(n);

                    more_p.Children.Add(Datatype());
                    n = match(Token_Class.Identifier);
                    if (n != null)
                        more_p.Children.Add(n);

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


            if (InputPointer < TokenStream.Count)
            {
                Node n = match(Token_Class.LCurlyBracket);
                if (n != null)
                    Function_Body.Children.Add(n);



                Function_Body.Children.Add(Statements());//statements -> State Statements | ɛ




                Function_Body.Children.Add(Return_Statement());



               n = match(Token_Class.RCurlyBracket);
                if (n != null)
                    Function_Body.Children.Add(n);

            }


            return Function_Body;

        }
        Node Function_Declaration()
        {
            Node Function_Declaration = new Node("Function_Declaration");
            if (InputPointer < TokenStream.Count)
            {



                Function_Declaration.Children.Add(Datatype());
                Function_Declaration.Children.Add(Function_Name());
                Node n = match(Token_Class.LBracket);
                if (n != null)
                    Function_Declaration.Children.Add(n);


                Function_Declaration.Children.Add(Parameters());//statements -> State Statements | ɛ

                n = match(Token_Class.RBracket);
                if (n != null)
                    Function_Declaration.Children.Add(n);


            }

            return Function_Declaration;

        }
        Node Function_Statement()
        {
            Node Function_Statement = new Node("Function_Statement");


            if (InputPointer < TokenStream.Count)
            {

                Function_Statement.Children.Add(Function_Declaration());
                Function_Statement.Children.Add(Function_Body());

            }


            return Function_Statement;

        }
        Node Main_Function()
        {
            Node Main_Function = new Node("Main_Function");

            if (InputPointer < TokenStream.Count)
            {


                Main_Function.Children.Add(Datatype());

                Node n = match(Token_Class.Main);
                if (n != null)
                    Main_Function.Children.Add(n);
               n = match(Token_Class.LBracket);
                if (n != null)
                    Main_Function.Children.Add(n);

               n = match(Token_Class.RBracket);
                if (n != null)
                    Main_Function.Children.Add(n);

                Main_Function.Children.Add(Function_Body());

            }


            return Main_Function;

        }

        Node If_Statement()
        {
            Node ifStmt = new Node("IfStatement");
            if (InputPointer < TokenStream.Count)
            {
                Node n = match(Token_Class.If);
                if (n != null)
                    ifStmt.Children.Add(n);
                ifStmt.Children.Add(Condition());
               n = match(Token_Class.Then);
                if (n != null)
                    ifStmt.Children.Add(n);
                ifStmt.Children.Add(Statements());
                ifStmt.Children.Add(IfTail());
               n = match(Token_Class.End);
                if (n != null)
                    ifStmt.Children.Add(n);
            }
            return ifStmt;
        }

        Node IfTail()
        {
            Node tail = new Node("IfTail");
            if (InputPointer < TokenStream.Count)
            {
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
            }


            return tail;
        }

        Node ElseIf_Statement()
        {
            Node elseif = new Node("ElseIfStatement");
            if (InputPointer < TokenStream.Count)
            {
                Node n = match(Token_Class.ElseIf);
                if (n != null)
                    elseif.Children.Add(n);
                elseif.Children.Add(Condition());
                n = match(Token_Class.Then);
                if (n != null)
                    elseif.Children.Add(n);
                elseif.Children.Add(Statements());
            }
            return elseif;
        }


        Node Else_Statement()
        {
            Node elseStmt = new Node("ElseStatement");
            if (InputPointer < TokenStream.Count)
            {
                Node n = match(Token_Class.Else);
                if (n != null)
                    elseStmt.Children.Add(n);
                elseStmt.Children.Add(Statements());
            }
            return elseStmt;
        }

        Node Write_Statement()
        {
            Node writeStmt = new Node("Write_Statement");
            if (InputPointer < TokenStream.Count)
            {
                Node n = match(Token_Class.Write);
                if (n != null)
                    writeStmt.Children.Add(n);

                if (TokenStream[InputPointer].token_type == Token_Class.Endl)
                {
                    n = match(Token_Class.Endl);
                    if (n != null)
                        writeStmt.Children.Add(n);
                }
                else
                {
                    writeStmt.Children.Add(Expression());
                }
                n = match(Token_Class.Semicolon);
                if (n != null)
                    writeStmt.Children.Add(n);

            }

            return writeStmt;
        }

        Node Read_Statement()
        {
            Node read = new Node("ReadStatement");
            if (InputPointer < TokenStream.Count)
            {
                Node n = match(Token_Class.Read);
                if (n != null)
                    read.Children.Add(n);
                n = match(Token_Class.Identifier);
                if (n != null)
                    read.Children.Add(n);
                n = match(Token_Class.Semicolon);
                if (n != null)
                    read.Children.Add(n);
            }
            return read;
        }

        Node Repeat_Statement()
        {

            Node repeat = new Node("RepeatStatement");
            if (InputPointer < TokenStream.Count)
            {
                Node n = match(Token_Class.Repeat);
                if (n != null)
                    repeat.Children.Add(n);
                repeat.Children.Add(Statements());
                n = match(Token_Class.Until);
                if (n != null)
                    repeat.Children.Add(n);
                repeat.Children.Add(Condition());
            }
            //repeat.Children.Add(match(Token_Class.Semicolon));
            return repeat;
        }

        Node Return_Statement()
        {
            Node ret = new Node("ReturnStatement");
            if (InputPointer < TokenStream.Count)
            {
                Node n = match(Token_Class.RETURN);
                if (n != null)
                    ret.Children.Add(n);
                ret.Children.Add(Expression());
                n = match(Token_Class.Semicolon);
                if (n != null)
                    ret.Children.Add(n);
            }
            return ret;
        }

        Node Condition()
        {
            Node condition = new Node("Condition");
            if (InputPointer < TokenStream.Count)
            {
                Node n = match(Token_Class.Identifier);
                if (n != null)
                    condition.Children.Add(n);
                condition.Children.Add(ConditionOp());
                condition.Children.Add(Term());
                condition.Children.Add(MoreConditions());
            }
            return condition;
        }

        Node MoreConditions()
        {
            Node more = new Node("MoreConditions");
            if (InputPointer < TokenStream.Count)
            {
                Token current = TokenStream[InputPointer];

                if (current.token_type == Token_Class.AndOp)
                {
                    Node n = match(Token_Class.AndOp);
                    if (n != null)
                        more.Children.Add(n);
                    n = match(Token_Class.Identifier);
                    if (n != null)
                        more.Children.Add(n);
                    more.Children.Add(ConditionOp());
                    more.Children.Add(Term());
                    more.Children.Add(MoreConditions());
                }
                else if (current.token_type == Token_Class.OrOp)
                {
                    Node n = match(Token_Class.OrOp);
                    if (n != null)
                        more.Children.Add(n);
                    n = match(Token_Class.Identifier);
                    if (n != null)
                        more.Children.Add(n);
                    more.Children.Add(ConditionOp());
                    more.Children.Add(Term());
                    more.Children.Add(MoreConditions());
                }
            }
            return more;
        }

        Node ConditionOp()
        {
            Node op = new Node("ConditionOp");
            if (InputPointer < TokenStream.Count)
            {
                Token current = TokenStream[InputPointer];

                if (current.token_type == Token_Class.EqualOp)
                {
                    Node n = match(Token_Class.EqualOp);
                    if (n != null)
                        op.Children.Add(n);
                }
                else if (current.token_type == Token_Class.LessThanOp)
                {
                    Node n = match(Token_Class.LessThanOp);
                    if (n != null)
                        op.Children.Add(n);
                }
                else if (current.token_type == Token_Class.GreaterThanOp)
                {
                    Node n = match(Token_Class.GreaterThanOp);
                    if (n != null)
                        op.Children.Add(n);
                }

                else
                {
                    Node n = match(Token_Class.NotEqualOp);
                    if (n != null)
                        op.Children.Add(n);
                }
            }

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
            if (InputPointer < TokenStream.Count)
            {

                Token_Class current = TokenStream[InputPointer].token_type;

                if (current == Token_Class.Identifier)
                {
                    if (InputPointer + 1 < TokenStream.Count && TokenStream[InputPointer + 1].token_type == Token_Class.LBracket)
                    {
                        statement.Children.Add(Function_Call());
                        Node n = match(Token_Class.Semicolon);
                        if (n != null)
                            statement.Children.Add(n);
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



            }

            return statement;
        }

        Node Function_Call()
        {
            Node functionCall = new Node("Function_Call");
            if (InputPointer < TokenStream.Count)
            {

                functionCall.Children.Add(Function_Name());
                Node n = match(Token_Class.LBracket);
                if (n != null)
                    functionCall.Children.Add(n);

                functionCall.Children.Add(Parameters_Call());
               n = match(Token_Class.RBracket);
                if (n != null)
                    functionCall.Children.Add(n);
            }

            //functionCall.Children.Add(match(Token_Class.Semicolon));


            return functionCall;
        }

        Node Assignment_Statement()
        {
            Node assign = new Node("Assignment_Statement");
            if (InputPointer < TokenStream.Count)
            {
                Node n = match(Token_Class.Identifier);
                if (n != null)
                    assign.Children.Add(n);
                n = match(Token_Class.AssignmentStatement);
                if (n != null)
                    assign.Children.Add(n);
                assign.Children.Add(Expression());
               n = match(Token_Class.Semicolon);
                if (n != null)
                    assign.Children.Add(n);
            }
            return assign;
        }

        Node Expression()
        {
            Node expr = new Node("Expression");


            if (InputPointer < TokenStream.Count)
            {
                if (TokenStream[InputPointer].token_type == Token_Class.String)
                {
                    Node n = match(Token_Class.String);
                    if (n != null)
                        expr.Children.Add(n);
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
            }

            return expr;
        }
        Node Term()
        {
            Node term = new Node("Term");
            if (InputPointer < TokenStream.Count)
            {
                if (TokenStream[InputPointer].token_type == Token_Class.Constant)
                {
                    Node n = match(Token_Class.Constant);
                    if (n != null)
                        term.Children.Add(n);
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
                        Node n = match(Token_Class.Identifier);
                        if (n != null)
                            term.Children.Add(n);
                    }
                }
            }
            return term;
        }

        Node Arithmetic_Operator()
        {
            Node op = new Node("Arithmetic_Operator");
            if (InputPointer < TokenStream.Count)
            {

                if (TokenStream[InputPointer].token_type == Token_Class.PlusOp)
                {
                    Node n = match(Token_Class.PlusOp);
                    if (n != null)
                        op.Children.Add(n);
                }
                else if (TokenStream[InputPointer].token_type == Token_Class.MinusOp)
                {
                    Node n = match(Token_Class.MinusOp);
                    if (n != null)
                        op.Children.Add(n);
                }
                else if (TokenStream[InputPointer].token_type == Token_Class.MultiplyOp)
                {
                    Node n = match(Token_Class.MultiplyOp);
                    if (n != null)
                        op.Children.Add(n);
                }
                else
                {
                    Node n = match(Token_Class.DivideOp);
                    if (n != null)
                        op.Children.Add(n);
                }

            }
            return op;
        }


        Node Equation() // (3*10)+5-6
        {
            Node eq = new Node("Equation");
            if (InputPointer < TokenStream.Count)
            {
                if (TokenStream[InputPointer].token_type == Token_Class.LBracket)
                {
                    Node n = match(Token_Class.LBracket);
                    if (n != null)
                        eq.Children.Add(n);
                    eq.Children.Add(Term());
                    eq.Children.Add(Operator_Equation());
                    n = match(Token_Class.RBracket);
                    if (n != null)
                        eq.Children.Add(n);
                    eq.Children.Add(Operator_Equation());
                }
                else
                {

                    eq.Children.Add(Term());
                    eq.Children.Add(Operator_Equation());
                }
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
            if (InputPointer < TokenStream.Count)
            {

                if (CheckArithmeticOperator())
                {
                    opEq.Children.Add(Arithmetic_Operator());

                    opEq.Children.Add(Equation());
                    //opEq.Children.Add(Operator_Equation());
                }
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

            if (InputPointer < TokenStream.Count)
            {
                if (TokenStream[InputPointer].token_type == Token_Class.Int)
                {
                    Node n = match(Token_Class.Int);
                    if (n != null)
                        datatype.Children.Add(n);
                }
                else if (TokenStream[InputPointer].token_type == Token_Class.String)
                {
                    Node n = match(Token_Class.String);
                    if (n != null)
                        datatype.Children.Add(n);

                }
                else
                {
                    Node n = match(Token_Class.Float);
                    if (n != null)
                        datatype.Children.Add(n);
                }
            }


            return datatype;
        }




        Node Function_Name()
        {
            Node fn_name = new Node("Function_Name");

            if (InputPointer < TokenStream.Count)
            {
                Node n = match(Token_Class.Identifier);
                if (n != null)
                    fn_name.Children.Add(n);


            }

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
