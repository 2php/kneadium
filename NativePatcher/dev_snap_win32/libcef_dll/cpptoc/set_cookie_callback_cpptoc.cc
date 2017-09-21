//---THIS-FILE-WAS-PATCHED , org=D:\projects\cef_binary_3.3071.1647.win32\cpptoc\set_cookie_callback_cpptoc.cc
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
// $hash=4a2ec304178530684d11e8b97ea2cfbd209d2f1e$
//

#include "libcef_dll/cpptoc/set_cookie_callback_cpptoc.h"

//---kneadium-ext-begin
#include "../myext/ExportFuncAuto.h"
#include "../myext/InternalHeaderForExportFunc.h"
//---kneadium-ext-end

namespace {

// MEMBER FUNCTIONS - Body may be edited by hand.

void CEF_CALLBACK
set_cookie_callback_on_complete(struct _cef_set_cookie_callback_t* self,
                                int success) {
  // AUTO-GENERATED CONTENT - DELETE THIS COMMENT BEFORE MODIFYING

  DCHECK(self);
  if (!self)
    return;

//---kneadium-ext-begin
auto me = CefSetCookieCallbackCppToC::Get(self);
const int CALLER_CODE=(CefSetCookieCallbackExt::_typeName << 16) | CefSetCookieCallbackExt::CefSetCookieCallbackExt_OnComplete_1;
auto m_callback= me->GetManagedCallBack(CALLER_CODE);
if(m_callback){
CefSetCookieCallbackExt::OnCompleteArgs args1(success);
m_callback(CALLER_CODE, &args1.arg);
 if (((args1.arg.myext_flags >> 21) & 1) == 1){
return;
}
}
//---kneadium-ext-end

  // Execute
  CefSetCookieCallbackCppToC::Get(self)->OnComplete(success ? true : false);
}

}  // namespace

// CONSTRUCTOR - Do not edit by hand.

CefSetCookieCallbackCppToC::CefSetCookieCallbackCppToC() {
  GetStruct()->on_complete = set_cookie_callback_on_complete;
}

template <>
CefRefPtr<CefSetCookieCallback> CefCppToCRefCounted<
    CefSetCookieCallbackCppToC,
    CefSetCookieCallback,
    cef_set_cookie_callback_t>::UnwrapDerived(CefWrapperType type,
                                              cef_set_cookie_callback_t* s) {
  NOTREACHED() << "Unexpected class type: " << type;
  return NULL;
}

#if DCHECK_IS_ON()
template <>
base::AtomicRefCount
    CefCppToCRefCounted<CefSetCookieCallbackCppToC,
                        CefSetCookieCallback,
                        cef_set_cookie_callback_t>::DebugObjCt = 0;
#endif

template <>
CefWrapperType CefCppToCRefCounted<CefSetCookieCallbackCppToC,
                                   CefSetCookieCallback,
                                   cef_set_cookie_callback_t>::kWrapperType =
    WT_SET_COOKIE_CALLBACK;
