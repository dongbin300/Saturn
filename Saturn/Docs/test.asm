byte var8, 12
word var16, 12345
dword var32, 12345678
aaa
aad 123
aam 128
aas
adc byte[eax], 125
adc byte[eax], 0x12
adc byte[ecx], bl
adc dword[eax], 125
adc dword[ecx], eax
adc var8, 123
adc var16, 1234
adc var32, 12345
adc al, 125
adc al, byte[ebp]
adc al, byte[eax]
adc al, var8
adc cl, 125
adc cl, byte[ebp]
adc cl, byte[eax]
adc cl, var8
adc ax, 1234
adc ax, word[ebp]
adc ax, word[eax]
adc ax, var16
adc cx, 1234
adc cx, word[ebp]
adc cx, word[eax]
adc cx, var16
adc eax, 12345678
adc eax, dword[ebp]
adc eax, dword[eax]
adc eax, var32
adc ecx, 12345678
adc ecx, dword[ebp]
adc ecx, dword[eax]
adc ecx, var32
add byte[eax], 123
add byte[eax], al
add word[eax], 123
add word[eax], 1234
add word[eax], ax
add dword[eax], 123
add dword[eax], 12345678
add dword[eax], eax
add var8, 123
add var16, 1234
add var32, 12345678
add al, 123
add al, byte[ebp]
add al, byte[eax]
add al, var8
add cl, 123
add cl, byte[ebp]
add cl, byte[eax]
add cl, var8
add ax, 1234
add ax, word[ebp]
add ax, word[eax]
add ax, var16
add cx, 1234
add cx, word[ebp]
add cx, word[eax]
add cx, var16
add eax, 12345678
add eax, dword[ebp]
add eax, dword[eax]
add eax, var32
add ecx, 12345678
add ecx, dword[ebp]
add ecx, dword[eax]
add ecx, var32
and byte[eax], 123
and byte[eax], al
and word[eax], 123
and word[eax], 1234
and word[eax], ax
and dword[eax], 123
and dword[eax], 12345678
and dword[eax], eax
and var8, 123
and var16, 1234
and var32, 12345678
and al, 123
and al, byte[eax]
and al, var8
and cl, 123
and cl, byte[eax]
and cl, var8
and ax, 123
and ax, 1234
and ax, word[eax]
and ax, var16
and cx, 123
and cx, 1234
and cx, word[eax]
and cx, var16
and eax, 123
and eax, 12345678
and eax, dword[eax]
and eax, var32
and ecx, 123
and ecx, 12345678
and ecx, dword[eax]
and ecx, var32
arpl word[ecx], sp
call dword[edi]
cdq
clc
cld
cli
cmc
cmp byte[eax], 123 ; 여기서부터 테스트 해야 합니다
cmp byte[eax], al
cmp word[eax], 123
cmp word[eax], 1234
cmp word[eax], ax
cmp dword[eax], 123
cmp dword[eax], 12345678
cmp dword[eax], eax
cmp var8, 123
cmp var16, 1234
cmp var32, 12345678
cmp al, 123
cmp al, byte[eax]
cmp al, var8
cmp cl, 123
cmp cl, byte[eax]
cmp cl, var8
cmp ax, 123
cmp ax, 1234
cmp ax, word[eax]
cmp ax, var16
cmp cx, 123
cmp cx, 1234
cmp cx, word[eax]
cmp cx, var16
cmp eax, 123
cmp eax, 12345678
cmp eax, dword[eax]
cmp eax, var32
cmp ecx, 123
cmp ecx, 12345678
cmp ecx, dword[eax]
cmp ecx, var32
cmps byte[eax]
cmps dword[eax]
cs
cwde
daa
das
dec byte[ecx]
dec dword[ecx]
dec edi
div var8
div var16
div var32
div dword[edx]
fwait
hlt
icebp
idiv var8
idiv var16
idiv var32
idiv dword[ecx]
imul var8
imul var16
imul var32
imul dword[edx]
in al, 0x12
in al, dx
in eax, 0x34
in eax, dx
inc byte[ecx]
inc dword[ecx]
inc edx
ins byte[edi], dx
ins dword[edi], dx
int 0x56
int3
into
iret
ja 0x78
jae 0x78
jb 0x78
jbe 0x78
je 0x78
jecxz 0x78
jg 0x78
jge 0x78
jl 0x78
jle 0x78
jmp 0x78
jmp dword[ecx]
jmp fword[edx]
jne 0x78
jno 0x78
jnp 0x78
jns 0x78
jo 0x78
jp 0x78
js 0x78
lahf
lea edx, eax
leave
lods al
lods eax
loop 0x34
loope 0x34
loopne 0x34
mov var8, 0x34
mov var16, 0x34
mov var32, 0x34
mov var32, al
mov var32, eax
mov byte[eax], al
mov word[eax], ax
mov dword[eax], ecx
mov al, 0x12
mov al, byte[ebp]
mov al, byte[eax]
mov al, var8
mov cl, 0x12
mov cl, byte[ebp]
mov cl, byte[eax]
mov cl, var8
mov ax, 0x1234
mov ax, word[ebp]
mov ax, word[eax]
mov ax, var16
mov cx, 0x1234
mov cx, word[ebp]
mov cx, word[eax]
mov cx, var16
mov eax, 0x12345678
mov eax, dword[ebp]
mov eax, dword[eax]
mov eax, var32
mov ecx, 0x12345678
mov ecx, dword[ebp]
mov ecx, dword[eax]
mov ecx, var32
mov ss, word[esi]
movs byte[ecx]
movs dword[ecx]
mul var8
mul var16
mul var32
mul dword[ecx]
neg var8
neg var16
neg var32
neg dword[ecx]
nop
not var8
not var16
not var32
not dword[ecx]
or byte[eax], 0x12
or byte[eax], al
or word[eax], 0x12
or word[eax], 0x1234
or word[eax], ax
or dword[eax], 0x12
or dword[eax], 0x12345678
or dword[eax], eax
or var8, 0x12
or var16, 0x1234
or var32, 0x12345678
or al, 0x12
or al, byte[eax]
or al, var8
or cl, 0x12
or cl, byte[eax]
or cl, var8
or ax, 0x1234
or ax, word[eax]
or ax, var16
or cx, 0x1234
or cx, word[eax]
or cx, var16
or eax, 0x12345678
or eax, dword[eax]
or eax, var32
or ecx, 0x12345678
or ecx, dword[eax]
or ecx, var32
out 0x12, al
out 0x12, eax
out dx, al
out dx, eax
outs dx, byte[esi]
outs dx, dword[esi]
pop dword[eax]
pop eax
pop es
pop ss
pop ds
popa
popf
push 0x12
push dword[eax]
push eax
push es
push cs
push ss
push ds
pusha
pushf
ret
retf
rcl byte[eax], cl
rcl byte[eax], 1
rcl dword[eax], cl
rcl dword[eax], 1
rcr byte[eax], cl
rcr byte[eax], 1
rcr dword[eax], cl
rcr dword[eax], 1
rol byte[eax], cl
rol byte[eax], 1
rol dword[eax], cl
rol dword[eax], 1
ror byte[eax], cl
ror byte[eax], 1
ror dword[eax], cl
ror dword[eax], 1
sahf
sbb byte[edx], 0x12
sbb byte[edx], ah
sbb word[edx], 0x12
sbb word[edx], 0x1234
sbb word[edx], dx
sbb dword[edx], 0x12
sbb dword[edx], 0x12345678
sbb dword[edx], edx
sbb var8, 0x12
sbb var16, 0x12
sbb var16, 0x1234
sbb var32, 0x12
sbb var32, 0x12345678
sbb al, 0x12
sbb al, byte[ebp]
sbb al, byte[edx]
sbb al, var8
sbb ah, 0x12
sbb ah, byte[ebp]
sbb ah, byte[edx]
sbb ah, var8
sbb ax, 0x1234
sbb ax, word[ebp]
sbb ax, word[edx]
sbb ax, var16
sbb dx, 0x1234
sbb dx, word[ebp]
sbb dx, word[edx]
sbb dx, var16
sbb eax, 0x12345678
sbb eax, dword[ebp]
sbb eax, dword[edx]
sbb eax, var32
sbb edx, 0x12345678
sbb edx, dword[ebp]
sbb edx, dword[edx]
sbb edx, var32
scas al
scas eax
shl byte[edx], cl
shl byte[edx], 1
shl dword[edx], cl
shl dword[edx], 1
shr byte[edx], cl
shr byte[edx], 1
shr dword[edx], cl
shr dword[edx], 1
stc
std
sti
stos byte[edi], al
stos dword[edi], eax
sub byte[edx], 0x12
sub byte[edx], ah
sub word[edx], 0x12
sub word[edx], 0x1234
sub word[edx], dx
sub dword[edx], 0x12
sub dword[edx], 0x12345678
sub dword[edx], edx
sub var8, 0x12
sub var16, 0x12
sub var16, 0x1234
sub var32, 0x12
sub var32, 0x12345678
sub al, 0x12
sub al, byte[ebp]
sub al, byte[edx]
sub al, var8
sub ah, 0x12
sub ah, byte[ebp]
sub ah, byte[edx]
sub ah, var8
sub ax, 0x1234
sub ax, word[ebp]
sub ax, word[edx]
sub ax, var16
sub dx, 0x1234
sub dx, word[ebp]
sub dx, word[edx]
sub dx, var16
sub eax, 0x12345678
sub eax, dword[ebp]
sub eax, dword[edx]
sub eax, var32
sub edx, 0x12345678
sub edx, dword[ebp]
sub edx, dword[edx]
sub edx, var32
test byte[edx], ah
test dword[edx], 0x12345678
test dword[edx], edx
test al, 0x12
test eax, 0x12345678
xchg byte[edx], ah
xchg dword[edx], edx
xchg edx, eax
xlat
xor byte[edx], 0x12
xor byte[edx], ah
xor word[edx], 0x12
xor word[edx], 0x1234
xor word[edx], dx
xor dword[edx], 0x12
xor dword[edx], 0x12345678
xor dword[edx], edx
xor var8, 0x12
xor var16, 0x12
xor var16, 0x1234
xor var32, 0x12
xor var32, 0x12345678
xor al, 0x12
xor al, byte[ebp]
xor al, byte[edx]
xor al, var8
xor ah, 0x12
xor ah, byte[ebp]
xor ah, byte[edx]
xor ah, var8
xor ax, 0x1234
xor ax, word[ebp]
xor ax, word[edx]
xor ax, var16
xor dx, 0x1234
xor dx, word[ebp]
xor dx, word[edx]
xor dx, var16
xor eax, 0x12345678
xor eax, dword[ebp]
xor eax, dword[edx]
xor eax, var32
xor edx, 0x12345678
xor edx, dword[ebp]
xor edx, dword[edx]
xor edx, var32
