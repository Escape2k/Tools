// MainFrm.h : CMainFrame 类的接口
//
#pragma once

#include "SpaceProject.h"
#include "SpaceItem.h"
#include "OutputPane.h"

#define VARIABLELEN_MAX 64000

class CMainFrame : public CXTPMDIFrameWnd
{
	DECLARE_DYNAMIC(CMainFrame)
public:
	enum{
		TIME_WATCH = 1,
		SHOW_ITEM = 9998,	// 显示变量
		PRINT = 9999,		// 打印信息
	};
	CMainFrame();
	CProgressCtrl	m_wndProgCtrl;							// 底边进度条
	CComVariant m_VarArray[VARIABLELEN_MAX];				// 建立一个VARIANT的数组，以备以后使用

protected:  // 控件条嵌入成员
	CXTPStatusBar	m_wndStatusBar;							// 状态栏
	CToolBar		m_wndToolBar;							// 工具条
	CAnimateCtrl	m_wndAnimCtrl;							// 状态栏显示动画效果

	HICON			m_hIcon;								// 图标
	CXTPDockingPaneManager m_paneManager;					// pan管理器
	CXTPTabClientWnd m_MTIClientWnd;						// 子窗体管理器
	Space::CSpaceOutput m_SpaceOutput;						// 文本输出框
	CXTPOfficeBorder<Space::CSpaceProject>	m_SpaceProject;	// 解决方案子pane
	CXTPOfficeBorder<Space::CSpaceItem>	m_SpaceItem;		// 变量组子pane
	bool			m_bLimitTimer;							// 是否限时定时器开启

public:
	Space::CSpaceProject* GetSpaceProject(){return &m_SpaceProject;}
	Space::CSpaceItem* GetSpaceItem(){return &m_SpaceItem;}
	Space::CSpaceOutput* GetOutPut(){return &m_SpaceOutput;}

public:
	virtual ~CMainFrame();
	virtual BOOL PreCreateWindow(CREATESTRUCT& cs);
	void UpdateTreeView();
	void SetTitle(CString title);
	void InitCommerTime();
	void SetTimeLimit(bool bLimit);				// 启动或停止时间限制
	void WaitPower2OpenProject(CString path);	// 得带获得权限信息后再打开工程
	bool AskAndProjClose();						// 询问并关闭工程

#ifdef _DEBUG
	virtual void AssertValid() const;
	virtual void Dump(CDumpContext& dc) const;
#endif

protected:
	bool CreateToolBar();
	bool CreateStatusBar();
	bool CreateSpaceDevice();
	void FillToStatusBar();						// 添加状态栏，图标、进度条、切换按钮、动态图片等
	void CompileRun(bool bRunServer = false);	// 启动编译，参数表示编译成功后是否要启动服务器运行该编译文件
	void ProjClose();							// 无声无息的关闭工程
	void SetTheme(int index);					// 设置窗口样式
	void SetPaneTheme(int index);				// 设置Pane样式

	afx_msg int OnCreate(LPCREATESTRUCT lpCreateStruct);
	afx_msg void OnCustomize();
	DECLARE_MESSAGE_MAP()

public:
	afx_msg void OnClose();
	afx_msg LRESULT OnDockingPaneNotify(WPARAM wParam, LPARAM lParam);
	afx_msg LRESULT OnTabbarMouseMsg(WPARAM wParam,LPARAM lParam);
	afx_msg void OnProjNew();					// 新建工程
	afx_msg void OnProjOpen();					// 打开工程
	afx_msg void OnProjSave();					// 保存工程
	afx_msg void OnProjBackUp();				// 备份工程
	afx_msg void OnProjClose();					// 关闭工程
	afx_msg void OnAddItem();
	afx_msg void OnRename();					// 重命名工程名
	afx_msg void OnProjInfo();
	afx_msg void OnSoftSet();
	afx_msg void OnFind();						// 查找功能
	afx_msg void OnTimer(UINT_PTR nIDEvent);
	afx_msg void OnShowViewProject();			// 显示工程试图
	afx_msg void OnShowViewItem();				// 显示变量组视图
	afx_msg void OnShowViewOutput();			// 显示输出栏视图
	afx_msg void OnKeyDown(UINT nChar, UINT nRepCnt, UINT nFlags);
	afx_msg void OnStartpage();
	afx_msg void OnHelpShow();

public:
	afx_msg void OnUpdateWithProj(CCmdUI *pCmdUI);		// 必须工程存在才使能的所有菜单项
	afx_msg void OnUpdateSoftSet(CCmdUI *pCmdUI);

public:
	DECLARE_EVENTSINK_MAP()
};

