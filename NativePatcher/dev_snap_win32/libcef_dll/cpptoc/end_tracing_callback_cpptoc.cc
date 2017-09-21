//---THIS-FILE-WAS-PATCHED , org=D:\projects\cef_binary_3.3071.1647.win32\cpptoc\end_tracing_callback_cpptoc.cc
// Copyright (c) 2017 The Chromium Embedded Framework Authors. All rights
// reserved. Use of this source code is governed by a BSD-style license that
// can be found in the LICENSE file.
//
// ---------------------------------------------------------------------------
//
// This file was generated by the CEF translator tool. If making changes by
// hand only do so within the body of existing method and function
// implementations. See the translator.README.txt file in the tools directory
// for more information.
//
// $hash=9a1b77364fd21f18c0bdbdec1b7abca0f6867323$
//

#include "libcef_dll/cpptoc/end_tracing_callback_cpptoc.h"

//---kneadium-ext-begin
#include "../myext/ExportFuncAuto.h"
#include "../myext/InternalHeaderForExportFunc.h"
//---kneadium-ext-end

namespace {

// MEMBER FUNCTIONS - Body may be edited by hand.

void CEF_CALLBACK end_tracing_callback_on_end_tracing_complete(
    struct _cef_end_tracing_callback_t* self,
    const cef_string_t* tracing_file) {
  // AUTO-GENERATED CONTENT - DELETE THIS COMMENT BEFORE MODIFYING

  DCHECK(self);
  if (!self)
    return;
  // Verify param: tracing_file; type: string_byref_const
  DCHECK(tracing_file);
  if (!tracing_file)
    return;

//---kneadium-ext-begin
auto me = CefEndTracingCallbackCppToC::Get(self);
const int CALLER_CODE=(CefEndTracingCallbackExt::_typeName << 16) | CefEndTracingCallbackExt::CefEndTracingCallbackExt_OnEndTracingComplete_1;
auto m_callback= me->GetManagedCallBack(CALLER_CODE);
if(m_callback){
CefString tmp_arg1 (tracing_file);
CefEndTracingCallbackExt::OnEndTracingCompleteArgs args1(tmp_arg1);
m_callback(CALLER_CODE, &args1.arg);
 if (((args1.arg.myext_flags >> 21) & 1) == 1){
return;
}
}
//---kneadium-ext-end

  // Execute
  CefEndTracingCallbackCppToC::Get(self)->OnEndTracingComplete(
      CefString(tracing_file));
}

}  // namespace

// CONSTRUCTOR - Do not edit by hand.

CefEndTracingCallbackCppToC::CefEndTracingCallbackCppToC() {
  GetStruct()->on_end_tracing_complete =
      end_tracing_callback_on_end_tracing_complete;
}

template <>
CefRefPtr<CefEndTracingCallback> CefCppToCRefCounted<
    CefEndTracingCallbackCppToC,
    CefEndTracingCallback,
    cef_end_tracing_callback_t>::UnwrapDerived(CefWrapperType type,
                                               cef_end_tracing_callback_t* s) {
  NOTREACHED() << "Unexpected class type: " << type;
  return NULL;
}

#if DCHECK_IS_ON()
template <>
base::AtomicRefCount
    CefCppToCRefCounted<CefEndTracingCallbackCppToC,
                        CefEndTracingCallback,
                        cef_end_tracing_callback_t>::DebugObjCt = 0;
#endif

template <>
CefWrapperType CefCppToCRefCounted<CefEndTracingCallbackCppToC,
                                   CefEndTracingCallback,
                                   cef_end_tracing_callback_t>::kWrapperType =
    WT_END_TRACING_CALLBACK;
