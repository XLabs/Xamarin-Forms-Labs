#pragma once

#include <windows.foundation.collections.h>

using namespace Platform;
using namespace Windows::Foundation::Collections;

namespace XLabs 
{
    namespace Platform 
    {
        namespace WinRT
        {
            public ref class FontService sealed
            {
            public:
                FontService();
                IIterable<String^>^ GetFontNames();
            };
        }
    }
}

