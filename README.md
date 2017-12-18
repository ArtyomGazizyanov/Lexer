Usage
Compiller.exe <inputFile>
Example: Compiller.exe input.txt
Example: Compiller.exe input.txt > out.txt


Main               ::= MainClass ( ClassDeclaration )* EOF

MainClass           ::= "class" Identifier "{" "public" "static" "void" "main" "(" "string" "[" "]" Identifier ")" "{" Statement "}" "}"

ClassDeclaration    ::= "class" Identifier "inherits" Identifier  "{" ( VarDeclaration )* ( MethodDeclaration )* "}"
                        | "class" Identifier "{" ( VarDeclaration )* ( MethodDeclaration )* "}"

VarDeclaration      ::= Type Identifier ";"

MethodDeclaration   ::= "public" Type Identifier "(" ( Type Identifier ( "," Type Identifier )* ) ")"
 "{" ( VarDeclaration )* ( Statement )* "return" Expression ";" "}"
                        | "public" Type Identifier "(" ")" "{" ( VarDeclaration )* ( Statement )* "return"
 Expression ";" "}"
|"private" Type Identifier "(" ( Type Identifier ( "," Type Identifier )* ) ")" "{" (
 VarDeclaration )* ( Statement )* "return" Expression ";" "}"
                        | "private" Type Identifier "(" ")" "{" ( VarDeclaration )* ( Statement )* "return"  
 Expression ";" "}"

Type                ::= "int" "[" "]"
                        | "boolean"
                        | "int"
                        | “string”

Statement           ::= "{" ( Statement )* "}"
                        | "if" "(" Expression ")" Statement "else" Statement
                        | "while" "(" Expression ")" Statement
                        | "System.out.println" "(" Expression ")" ";"
                        | Identifier "=" Expression ";"
                        | Identifier "[" Expression "]" "=" Expression ";"

Expression          ::= Expression ( "&&" | "<" | "+" | "-" | "*" ) Expression
                        | Expression "[" Expression "]"
                        | Expression "." "length"
                        | Expression "." Identifier "(" ( Expression ( "," Expression )* )? ")"
                        | Expression "." Identifier "(" ")"
                        | "true"
                        | "false"
                        | Identifier
                        | "this"
                        | "new" "int" "[" Expression "]"
                        | "new" Identifier "(" ")"
                        | "!" Expression
                        | "(" Expression ")"
                        | <INTEGER_IdentifierL>


Digit		::= "0"|"1"|"2"|"3"|"4"|"5"|"6"|"7"|"8"|"9".

Identifier          ::= Letter
|Identifier Digit
|Identifier Letter

# Lexer