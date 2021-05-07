.model flat, C

Hook PROTO stdcall :DWORD
PUBLIC gateway

.code

gateway PROC C
	push ecx
	push eax		
	push esi					; esi contains the text and should be the arg
	call Hook					; call our hook
	pop eax
	pop ecx
	mov [ebp-120h], word ptr 5	; original opcodes
	ret							; return
gateway ENDP

END