#pragma once
#include "dllmain.h"

namespace pipe
{
	extern HANDLE hPipe;

	HANDLE CreateAndConnectPipe(LPWSTR pipeName);
	void WINAPI SendText(const char* text);
	void Disconnect();
}