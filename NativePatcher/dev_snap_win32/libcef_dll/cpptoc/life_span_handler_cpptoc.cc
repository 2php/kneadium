//---THIS-FILE-WAS-PATCHED , org=D:\projects\cef_binary_3.3071.1647.win32\cpptoc\life_span_handler_cpptoc.cc
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
// $hash=a2d97e44c5404e9694b436eb590979759cc84972$
//

#include "libcef_dll/cpptoc/life_span_handler_cpptoc.h"
#include "libcef_dll/cpptoc/client_cpptoc.h"
#include "libcef_dll/ctocpp/browser_ctocpp.h"
#include "libcef_dll/ctocpp/frame_ctocpp.h"

//---kneadium-ext-begin
#include "../myext/ExportFuncAuto.h"
#include "../myext/InternalHeaderForExportFunc.h"
//---kneadium-ext-end

namespace {

	// MEMBER FUNCTIONS - Body may be edited by hand.

	int CEF_CALLBACK life_span_handler_on_before_popup(
		struct _cef_life_span_handler_t* self,
		cef_browser_t* browser,
		cef_frame_t* frame,
		const cef_string_t* target_url,
		const cef_string_t* target_frame_name,
		cef_window_open_disposition_t target_disposition,
		int user_gesture,
		const struct _cef_popup_features_t* popupFeatures,
		cef_window_info_t* windowInfo,
		cef_client_t** client,
		struct _cef_browser_settings_t* settings,
		int* no_javascript_access) {
		// AUTO-GENERATED CONTENT - DELETE THIS COMMENT BEFORE MODIFYING

		DCHECK(self);
		if (!self)
			return 0;
		// Verify param: browser; type: refptr_diff
		DCHECK(browser);
		if (!browser)
			return 0;
		// Verify param: frame; type: refptr_diff
		DCHECK(frame);
		if (!frame)
			return 0;
		// Verify param: popupFeatures; type: struct_byref_const
		DCHECK(popupFeatures);
		if (!popupFeatures)
			return 0;
		// Verify param: windowInfo; type: struct_byref
		DCHECK(windowInfo);
		if (!windowInfo)
			return 0;
		// Verify param: client; type: refptr_same_byref
		DCHECK(client);
		if (!client)
			return 0;
		// Verify param: settings; type: struct_byref
		DCHECK(settings);
		if (!settings)
			return 0;
		// Verify param: no_javascript_access; type: bool_byaddr
		DCHECK(no_javascript_access);
		if (!no_javascript_access)
			return 0;
		// Unverified params: target_url, target_frame_name

		// Translate param: popupFeatures; type: struct_byref_const
		CefPopupFeatures popupFeaturesObj;
		if (popupFeatures)
			popupFeaturesObj.Set(*popupFeatures, false);
		// Translate param: windowInfo; type: struct_byref
		CefWindowInfo windowInfoObj;
		if (windowInfo)
			windowInfoObj.AttachTo(*windowInfo);
		// Translate param: client; type: refptr_same_byref
		CefRefPtr<CefClient> clientPtr;
		if (client && *client)
			clientPtr = CefClientCppToC::Unwrap(*client);
		CefClient* clientOrig = clientPtr.get();
		// Translate param: settings; type: struct_byref
		CefBrowserSettings settingsObj;
		if (settings)
			settingsObj.AttachTo(*settings);
		// Translate param: no_javascript_access; type: bool_byaddr
		bool no_javascript_accessBool =
			(no_javascript_access && *no_javascript_access) ? true : false;

		//---kneadium-ext-begin
		auto me = CefLifeSpanHandlerCppToC::Get(self);
		const int CALLER_CODE = (CefLifeSpanHandlerExt::_typeName << 16) | CefLifeSpanHandlerExt::CefLifeSpanHandlerExt_OnBeforePopup_1;
		auto m_callback = me->GetManagedCallBack(CALLER_CODE);
		if (m_callback) {
			CefString tmp_arg3(target_url);
			CefString tmp_arg4(target_frame_name);
			CefLifeSpanHandlerExt::OnBeforePopupArgs args1(browser, frame, tmp_arg3, tmp_arg4, target_disposition, user_gesture, &popupFeaturesObj, &windowInfoObj, &clientPtr, &settingsObj, &no_javascript_accessBool);
			m_callback(CALLER_CODE, &args1.arg);
			if (((args1.arg.myext_flags >> 21) & 1) == 1) {
				// Restore param: windowInfo; type: struct_byref
				if (windowInfo)
					windowInfoObj.DetachTo(*windowInfo);

				// Restore param: client; type: refptr_same_byref
				if (client) {
					if (clientPtr.get()) {
						if (clientPtr.get() != clientOrig) {
							*client = CefClientCppToC::Wrap(clientPtr);
						}
					}
					else {
						*client = NULL;
					}
				}

				// Restore param: settings; type: struct_byref
				if (settings)
					settingsObj.DetachTo(*settings);

				// Restore param: no_javascript_access; type: bool_byaddr
				if (no_javascript_access)
					*no_javascript_access = no_javascript_accessBool ? true : false;


				return args1.arg.myext_ret_value;
			}
		}
		//---kneadium-ext-end

		  // Execute
		bool _retval = CefLifeSpanHandlerCppToC::Get(self)->OnBeforePopup(
			CefBrowserCToCpp::Wrap(browser), CefFrameCToCpp::Wrap(frame),
			CefString(target_url), CefString(target_frame_name), target_disposition,
			user_gesture ? true : false, popupFeaturesObj, windowInfoObj, clientPtr,
			settingsObj, &no_javascript_accessBool);

		// Restore param: windowInfo; type: struct_byref
		if (windowInfo)
			windowInfoObj.DetachTo(*windowInfo);
		// Restore param: client; type: refptr_same_byref
		if (client) {
			if (clientPtr.get()) {
				if (clientPtr.get() != clientOrig) {
					*client = CefClientCppToC::Wrap(clientPtr);
				}
			}
			else {
				*client = NULL;
			}
		}
		// Restore param: settings; type: struct_byref
		if (settings)
			settingsObj.DetachTo(*settings);
		// Restore param: no_javascript_access; type: bool_byaddr
		if (no_javascript_access)
			*no_javascript_access = no_javascript_accessBool ? true : false;

		// Return type: bool
		return _retval;
	}

	void CEF_CALLBACK
		life_span_handler_on_after_created(struct _cef_life_span_handler_t* self,
			cef_browser_t* browser) {
		// AUTO-GENERATED CONTENT - DELETE THIS COMMENT BEFORE MODIFYING

		DCHECK(self);
		if (!self)
			return;
		// Verify param: browser; type: refptr_diff
		DCHECK(browser);
		if (!browser)
			return;

		//---kneadium-ext-begin
		auto me = CefLifeSpanHandlerCppToC::Get(self);
		const int CALLER_CODE = (CefLifeSpanHandlerExt::_typeName << 16) | CefLifeSpanHandlerExt::CefLifeSpanHandlerExt_OnAfterCreated_2;
		auto m_callback = me->GetManagedCallBack(CALLER_CODE);
		if (m_callback) {
			CefLifeSpanHandlerExt::OnAfterCreatedArgs args1(browser);
			m_callback(CALLER_CODE, &args1.arg);
			if (((args1.arg.myext_flags >> 21) & 1) == 1) {
				return;
			}
		}
		//---kneadium-ext-end

		  // Execute
		CefLifeSpanHandlerCppToC::Get(self)->OnAfterCreated(
			CefBrowserCToCpp::Wrap(browser));
	}

	int CEF_CALLBACK
		life_span_handler_do_close(struct _cef_life_span_handler_t* self,
			cef_browser_t* browser) {
		// AUTO-GENERATED CONTENT - DELETE THIS COMMENT BEFORE MODIFYING

		DCHECK(self);
		if (!self)
			return 0;
		// Verify param: browser; type: refptr_diff
		DCHECK(browser);
		if (!browser)
			return 0;

		//---kneadium-ext-begin
		auto me = CefLifeSpanHandlerCppToC::Get(self);
		const int CALLER_CODE = (CefLifeSpanHandlerExt::_typeName << 16) | CefLifeSpanHandlerExt::CefLifeSpanHandlerExt_DoClose_3;
		auto m_callback = me->GetManagedCallBack(CALLER_CODE);
		if (m_callback) {
			CefLifeSpanHandlerExt::DoCloseArgs args1(browser);
			m_callback(CALLER_CODE, &args1.arg);
			if (((args1.arg.myext_flags >> 21) & 1) == 1) {
				return args1.arg.myext_ret_value;
			}
		}
		//---kneadium-ext-end

		  // Execute
		bool _retval = CefLifeSpanHandlerCppToC::Get(self)->DoClose(
			CefBrowserCToCpp::Wrap(browser));

		// Return type: bool
		return _retval;
	}

	void CEF_CALLBACK
		life_span_handler_on_before_close(struct _cef_life_span_handler_t* self,
			cef_browser_t* browser) {
		// AUTO-GENERATED CONTENT - DELETE THIS COMMENT BEFORE MODIFYING

		DCHECK(self);
		if (!self)
			return;
		// Verify param: browser; type: refptr_diff
		DCHECK(browser);
		if (!browser)
			return;

		//---kneadium-ext-begin
		auto me = CefLifeSpanHandlerCppToC::Get(self);
		const int CALLER_CODE = (CefLifeSpanHandlerExt::_typeName << 16) | CefLifeSpanHandlerExt::CefLifeSpanHandlerExt_OnBeforeClose_4;
		auto m_callback = me->GetManagedCallBack(CALLER_CODE);
		if (m_callback) {
			CefLifeSpanHandlerExt::OnBeforeCloseArgs args1(browser);
			m_callback(CALLER_CODE, &args1.arg);
			if (((args1.arg.myext_flags >> 21) & 1) == 1) {
				return;
			}
		}
		//---kneadium-ext-end

		  // Execute
		CefLifeSpanHandlerCppToC::Get(self)->OnBeforeClose(
			CefBrowserCToCpp::Wrap(browser));
	}

}  // namespace

// CONSTRUCTOR - Do not edit by hand.

CefLifeSpanHandlerCppToC::CefLifeSpanHandlerCppToC() {
	GetStruct()->on_before_popup = life_span_handler_on_before_popup;
	GetStruct()->on_after_created = life_span_handler_on_after_created;
	GetStruct()->do_close = life_span_handler_do_close;
	GetStruct()->on_before_close = life_span_handler_on_before_close;
}

template <>
CefRefPtr<CefLifeSpanHandler> CefCppToCRefCounted<
	CefLifeSpanHandlerCppToC,
	CefLifeSpanHandler,
	cef_life_span_handler_t>::UnwrapDerived(CefWrapperType type,
		cef_life_span_handler_t* s) {
	NOTREACHED() << "Unexpected class type: " << type;
	return NULL;
}

#if DCHECK_IS_ON()
template <>
base::AtomicRefCount CefCppToCRefCounted<CefLifeSpanHandlerCppToC,
	CefLifeSpanHandler,
	cef_life_span_handler_t>::DebugObjCt =
	0;
#endif

template <>
CefWrapperType CefCppToCRefCounted<CefLifeSpanHandlerCppToC,
	CefLifeSpanHandler,
	cef_life_span_handler_t>::kWrapperType =
	WT_LIFE_SPAN_HANDLER;
