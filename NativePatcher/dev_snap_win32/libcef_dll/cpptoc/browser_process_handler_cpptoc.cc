//---THIS-FILE-WAS-PATCHED , org=D:\projects\cef_binary_3.3071.1647.win32\cpptoc\browser_process_handler_cpptoc.cc
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
// $hash=81332687e151af6933a729c1456dd4b3a64f82df$
//

#include "libcef_dll/cpptoc/browser_process_handler_cpptoc.h"
#include "libcef_dll/cpptoc/print_handler_cpptoc.h"
#include "libcef_dll/ctocpp/command_line_ctocpp.h"
#include "libcef_dll/ctocpp/list_value_ctocpp.h"

//---kneadium-ext-begin
#include "../myext/ExportFuncAuto.h"
#include "../myext/InternalHeaderForExportFunc.h"
//---kneadium-ext-end

namespace {

// MEMBER FUNCTIONS - Body may be edited by hand.

void CEF_CALLBACK browser_process_handler_on_context_initialized(
    struct _cef_browser_process_handler_t* self) {
  // AUTO-GENERATED CONTENT - DELETE THIS COMMENT BEFORE MODIFYING

  DCHECK(self);
  if (!self)
    return;

//---kneadium-ext-begin
auto me = CefBrowserProcessHandlerCppToC::Get(self);
const int CALLER_CODE=(CefBrowserProcessHandlerExt::_typeName << 16) | CefBrowserProcessHandlerExt::CefBrowserProcessHandlerExt_OnContextInitialized_1;
auto m_callback= me->GetManagedCallBack(CALLER_CODE);
if(m_callback){
CefBrowserProcessHandlerExt::OnContextInitializedArgs args1;
m_callback(CALLER_CODE, &args1.arg);
 if (((args1.arg.myext_flags >> 21) & 1) == 1){
return;
}
}
//---kneadium-ext-end

  // Execute
  CefBrowserProcessHandlerCppToC::Get(self)->OnContextInitialized();
}

void CEF_CALLBACK browser_process_handler_on_before_child_process_launch(
    struct _cef_browser_process_handler_t* self,
    struct _cef_command_line_t* command_line) {
  // AUTO-GENERATED CONTENT - DELETE THIS COMMENT BEFORE MODIFYING

  DCHECK(self);
  if (!self)
    return;
  // Verify param: command_line; type: refptr_diff
  DCHECK(command_line);
  if (!command_line)
    return;

//---kneadium-ext-begin
auto me = CefBrowserProcessHandlerCppToC::Get(self);
const int CALLER_CODE=(CefBrowserProcessHandlerExt::_typeName << 16) | CefBrowserProcessHandlerExt::CefBrowserProcessHandlerExt_OnBeforeChildProcessLaunch_2;
auto m_callback= me->GetManagedCallBack(CALLER_CODE);
if(m_callback){
CefBrowserProcessHandlerExt::OnBeforeChildProcessLaunchArgs args1(command_line);
m_callback(CALLER_CODE, &args1.arg);
 if (((args1.arg.myext_flags >> 21) & 1) == 1){
return;
}
}
//---kneadium-ext-end

  // Execute
  CefBrowserProcessHandlerCppToC::Get(self)->OnBeforeChildProcessLaunch(
      CefCommandLineCToCpp::Wrap(command_line));
}

void CEF_CALLBACK browser_process_handler_on_render_process_thread_created(
    struct _cef_browser_process_handler_t* self,
    struct _cef_list_value_t* extra_info) {
  // AUTO-GENERATED CONTENT - DELETE THIS COMMENT BEFORE MODIFYING

  DCHECK(self);
  if (!self)
    return;
  // Verify param: extra_info; type: refptr_diff
  DCHECK(extra_info);
  if (!extra_info)
    return;

//---kneadium-ext-begin
auto me = CefBrowserProcessHandlerCppToC::Get(self);
const int CALLER_CODE=(CefBrowserProcessHandlerExt::_typeName << 16) | CefBrowserProcessHandlerExt::CefBrowserProcessHandlerExt_OnRenderProcessThreadCreated_3;
auto m_callback= me->GetManagedCallBack(CALLER_CODE);
if(m_callback){
CefBrowserProcessHandlerExt::OnRenderProcessThreadCreatedArgs args1(extra_info);
m_callback(CALLER_CODE, &args1.arg);
 if (((args1.arg.myext_flags >> 21) & 1) == 1){
return;
}
}
//---kneadium-ext-end

  // Execute
  CefBrowserProcessHandlerCppToC::Get(self)->OnRenderProcessThreadCreated(
      CefListValueCToCpp::Wrap(extra_info));
}

struct _cef_print_handler_t* CEF_CALLBACK
browser_process_handler_get_print_handler(
    struct _cef_browser_process_handler_t* self) {
  // AUTO-GENERATED CONTENT - DELETE THIS COMMENT BEFORE MODIFYING

  DCHECK(self);
  if (!self)
    return NULL;

//---kneadium-ext-begin
auto me = CefBrowserProcessHandlerCppToC::Get(self);
const int CALLER_CODE=(CefBrowserProcessHandlerExt::_typeName << 16) | CefBrowserProcessHandlerExt::CefBrowserProcessHandlerExt_GetPrintHandler_4;
auto m_callback= me->GetManagedCallBack(CALLER_CODE);
if(m_callback){
CefBrowserProcessHandlerExt::GetPrintHandlerArgs args1;
m_callback(CALLER_CODE, &args1.arg);
 if (((args1.arg.myext_flags >> 21) & 1) == 1){
 return CefPrintHandlerCppToC::Wrap(args1.arg.myext_ret_value);
}
}
//---kneadium-ext-end

  // Execute
  CefRefPtr<CefPrintHandler> _retval =
      CefBrowserProcessHandlerCppToC::Get(self)->GetPrintHandler();

  // Return type: refptr_same
  return CefPrintHandlerCppToC::Wrap(_retval);
}

void CEF_CALLBACK browser_process_handler_on_schedule_message_pump_work(
    struct _cef_browser_process_handler_t* self,
    int64 delay_ms) {
  // AUTO-GENERATED CONTENT - DELETE THIS COMMENT BEFORE MODIFYING

  DCHECK(self);
  if (!self)
    return;

//---kneadium-ext-begin
auto me = CefBrowserProcessHandlerCppToC::Get(self);
const int CALLER_CODE=(CefBrowserProcessHandlerExt::_typeName << 16) | CefBrowserProcessHandlerExt::CefBrowserProcessHandlerExt_OnScheduleMessagePumpWork_5;
auto m_callback= me->GetManagedCallBack(CALLER_CODE);
if(m_callback){
CefBrowserProcessHandlerExt::OnScheduleMessagePumpWorkArgs args1(delay_ms);
m_callback(CALLER_CODE, &args1.arg);
 if (((args1.arg.myext_flags >> 21) & 1) == 1){
return;
}
}
//---kneadium-ext-end

  // Execute
  CefBrowserProcessHandlerCppToC::Get(self)->OnScheduleMessagePumpWork(
      delay_ms);
}

}  // namespace

// CONSTRUCTOR - Do not edit by hand.

CefBrowserProcessHandlerCppToC::CefBrowserProcessHandlerCppToC() {
  GetStruct()->on_context_initialized =
      browser_process_handler_on_context_initialized;
  GetStruct()->on_before_child_process_launch =
      browser_process_handler_on_before_child_process_launch;
  GetStruct()->on_render_process_thread_created =
      browser_process_handler_on_render_process_thread_created;
  GetStruct()->get_print_handler = browser_process_handler_get_print_handler;
  GetStruct()->on_schedule_message_pump_work =
      browser_process_handler_on_schedule_message_pump_work;
}

template <>
CefRefPtr<CefBrowserProcessHandler> CefCppToCRefCounted<
    CefBrowserProcessHandlerCppToC,
    CefBrowserProcessHandler,
    cef_browser_process_handler_t>::UnwrapDerived(CefWrapperType type,
                                                  cef_browser_process_handler_t*
                                                      s) {
  NOTREACHED() << "Unexpected class type: " << type;
  return NULL;
}

#if DCHECK_IS_ON()
template <>
base::AtomicRefCount
    CefCppToCRefCounted<CefBrowserProcessHandlerCppToC,
                        CefBrowserProcessHandler,
                        cef_browser_process_handler_t>::DebugObjCt = 0;
#endif

template <>
CefWrapperType
    CefCppToCRefCounted<CefBrowserProcessHandlerCppToC,
                        CefBrowserProcessHandler,
                        cef_browser_process_handler_t>::kWrapperType =
        WT_BROWSER_PROCESS_HANDLER;
