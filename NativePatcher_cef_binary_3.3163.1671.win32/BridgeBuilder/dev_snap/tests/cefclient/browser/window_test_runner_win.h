//###_ORIGINAL D:\projects\cef_binary_3.3163.1671.win32\tests\cefclient\browser//window_test_runner_win.h
// Copyright (c) 2016 The Chromium Embedded Framework Authors. All rights
// reserved. Use of this source code is governed by a BSD-style license that
// can be found in the LICENSE file.

#ifndef CEF_TESTS_CEFCLIENT_BROWSER_WINDOW_TEST_RUNNER_WIN_H_
#define CEF_TESTS_CEFCLIENT_BROWSER_WINDOW_TEST_RUNNER_WIN_H_
#pragma once

#include "tests/cefclient/browser/window_test_runner.h"

namespace client {
namespace window_test {

//###_BEGIN
#include "tests/cefclient/myext/mycef_buildconfig.h"
#if BUILD_TEST
//###_END

// Windows platform implementation. Methods are safe to call on any browser
// process thread.
class WindowTestRunnerWin : public WindowTestRunner {
 public:
  WindowTestRunnerWin();

  void SetPos(CefRefPtr<CefBrowser> browser,
              int x,
              int y,
              int width,
              int height) OVERRIDE;
  void Minimize(CefRefPtr<CefBrowser> browser) OVERRIDE;
  void Maximize(CefRefPtr<CefBrowser> browser) OVERRIDE;
  void Restore(CefRefPtr<CefBrowser> browser) OVERRIDE;
};

//###_BEGIN
#endif
//###_END

}  // namespace window_test
}  // namespace client

#endif  // CEF_TESTS_CEFCLIENT_BROWSER_WINDOW_TEST_RUNNER_WIN_H_
