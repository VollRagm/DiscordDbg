#include "dllmain.h"
#include "pipe.h"
#include "hook.h"

DWORD WINAPI MainThread(HMODULE mod)
{
    pipe::hPipe = pipe::CreateAndConnectPipe((LPWSTR)L"\\\\.\\pipe\\dbgview");
    if (pipe::hPipe == INVALID_HANDLE_VALUE)
        FreeLibraryAndExitThread(mod, -1);
    
    Sleep(100);
    PlaceHook();
    return 0;
}


BOOL APIENTRY DllMain(HMODULE hModule, DWORD  ul_reason_for_call, LPVOID lpReserved)
{
    if (ul_reason_for_call == DLL_PROCESS_ATTACH)
        CreateThread(0, 0, (LPTHREAD_START_ROUTINE)MainThread, hModule, 0, 0);

    return TRUE;
}