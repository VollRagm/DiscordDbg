#include "hook.h"
#include "pipe.h"

extern "C" void __stdcall Hook(char* text)
{
    pipe::SendText(text);
}

void PlaceHook()
{
    HMODULE base = GetModuleHandle(NULL);
    BYTE* hookAddr = (BYTE*)base + 0xEEBE;

    BYTE hook_shellcode[] = {
       0xB8, 0x00, 0x00, 0x00, 0x00,    //mov rax
       0xFF, 0xD0,                      //call rax
       0x90, 0x90, 0x90                 //nop nop nop
    };

    DWORD hookFuncAddy = (DWORD)gateway;
    memcpy(hook_shellcode + 1, &hookFuncAddy, sizeof(DWORD));

    DWORD oldProtect = PAGE_EXECUTE_READ;

    VirtualProtect(hookAddr, sizeof(hook_shellcode), PAGE_EXECUTE_READWRITE, &oldProtect);
    memcpy(hookAddr, &hook_shellcode, sizeof(hook_shellcode));
    VirtualProtect(hookAddr, sizeof(hook_shellcode), oldProtect, &oldProtect);
}