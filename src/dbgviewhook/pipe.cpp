#include "pipe.h"

namespace pipe
{
    HANDLE hPipe;

    HANDLE CreateAndConnectPipe(LPWSTR pipeName)
    {
        HANDLE pipeHandle;
        pipeHandle = CreateNamedPipe(pipeName, PIPE_ACCESS_DUPLEX,
            PIPE_TYPE_BYTE | PIPE_READMODE_BYTE | PIPE_WAIT,
            1, 1024 * 16, 1024, NMPWAIT_USE_DEFAULT_WAIT, nullptr);

        std::cout << std::hex << GetLastError() << std::endl;

        if (pipeHandle == INVALID_HANDLE_VALUE) return pipeHandle;
        bool success = ConnectNamedPipe(pipeHandle, nullptr);
        if (!success) return INVALID_HANDLE_VALUE;
        hPipe = pipeHandle;
        return pipeHandle;
    }

    void Disconnect()
    {
        DisconnectNamedPipe(hPipe);
        CloseHandle(hPipe);
    }

    void WINAPI SendText(const char* text)
    {
        char* _text = (char*)text;
        int length = strlen(_text);
        bool success = WriteFile(hPipe, _text, length, nullptr, nullptr);
    }

}