.386                            ; Tells MASM to use Intel 80386 instruction set.

.model flat,stdcall             ; Flat memory model

option casemap:none             ; Treat labels as case-sensitive

include    \masm32\include\windows.inc
include    \masm32\include\kernel32.inc
includelib    \masm32\lib\kernel32.lib

include    \masm32\include\user32.inc
includelib    \masm32\lib\user32.lib

.data                           ; Begin initialized data segment

       MsgBoxCaption db "Win32 Assembly Programming",0
       MsgBoxText db "Hello World!!!Welcome to ASM Programming under CLR",0

.code                            ; Beginning of code

start:                           ; Entry point of the code
        invoke MessageBox, NULL, addr MsgBoxText, addr MsgBoxCaption, MB_OK
        invoke ExitProcess, NULL
        end start