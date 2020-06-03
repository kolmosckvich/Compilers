grammar ClojureObr;

file: expr * EOF ;

define: '(' DEF symbol expr ')' ;

condition
    : '(' COND expr expr expr ')' 
    | '(' COND expr expr ')'
    ;

fun : '(' FUN '[' symbol* ']' expressions ')' ;

expr
    : define
    | condition
    | fun
    | literal
    | list
    | vector
    | map
    ;

expressions: expr* ;

list: '(' expressions ')' ;

vector: '[' expressions ']' ;

map: '{' (keyword expr)* '}' ;

literal
    : string
    | number
    | character
    | nil
    | bool
    | keyword
    | symbol
    ;

string: STRING ;

bool: BOOLEAN ;

float: FLOAT ;

int: INT ;

number
    : int
    | float
    ;

character
    : char
    | named_char
    ;

named_char: CHAR_NAMED ;

char 
    : CHAR_ANY 
    | CHAR_NAMED
    ;

nil: NIL;

keyword: ':' symbol ;

symbol
    : SYMBOL 
    | DEF
    | COND
    | FUN
    ;

// Lexer

DEF : 'def' ;

COND : 'if' ;

FUN : 'fn' ;

STRING : '"' ( ~'"' | '\\' '"' )* '"' ;

FLOAT
    : '-'? [0-9]+ FLOAT_TAIL ;

fragment
FLOAT_TAIL
    : FLOAT_DECIMAL FLOAT_EXP
    | FLOAT_DECIMAL
    | FLOAT_EXP
    ;

fragment
FLOAT_DECIMAL : '.' [0-9]+ ;

fragment
FLOAT_EXP : [eE] '-'? [0-9]+ ;

INT: '-'? [0-9]+ ;

CHAR_NAMED
    : '\\' ( 'newline'
           | 'space'
           | 'tab'
           ) ;
CHAR_ANY
    : '\\' . ;

NIL : 'nil';

BOOLEAN : 'true' | 'false' ;

SYMBOL : NAME;

fragment
NAME : SYMBOL_HEAD SYMBOL_BODY* ;

fragment
SYMBOL_HEAD 
        : [a-zA-Z_]
        | SYMBOL_SPEC
        ;

fragment
SYMBOL_BODY
        : [0-9]
        | [a-zA-Z_]
        | SYMBOL_SPEC
        ;

fragment
SYMBOL_SPEC
        : '+'
        | '-'
        | '*'
        | '//'
        | '!'
        | '?'
        | '.'
        | '_'
        ;


// Discard


fragment
TS : [ \n\r\t,] ;

fragment
COMMENT: ';' ~[\r\n]* ;

TRASH : ( TS | COMMENT ) -> channel(HIDDEN);