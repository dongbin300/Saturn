dword result, 0


dword babo1, 10
dword babo2, 20
dword babo3, 30
dword babo4, 40
dword babo5, 50
dword babo6, 60
dword babo7, 70
dword babo8, 80
dword babo9, 90
dword babo10, 100
dword babo11, 110
dword babo0, 1

prcd test_nodap

nodap: ; == while (true)
mov eax, babo1
add eax, babo2
add eax, 3
add eax, 7
cmp eax, 40
jne nodap

mov eax, 91241h

prcd end

prcd main
call test_nodap
; printf eax
prcd end
