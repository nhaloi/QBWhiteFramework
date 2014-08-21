﻿using System;
using FrameworkLibraries.Utils;
using System.Windows.Automation;
using System.Windows.Forms;
using FrameworkLibraries.ActionLibs.QBDT;
using TestStack.White.UIItems.WindowItems;
using System.Threading;
using TestStack.White.UIItems.Finders;
using FrameworkLibraries.ActionLibs.QBDT.WhiteAPI;
using FrameworkLibraries;
using System.Collections.Generic;
using TestStack.White.UIItems;
using Xunit;
using Xunit.Extensions;
using TestStack.BDDfy;
using FrameworkLibraries.AppLibs.QBDT.WhiteAPI;
using BATS.DATA;

namespace BATS.Tests
{
    public class OpenQbBackupFile
    {
        public TestStack.White.Application qbApp = null;
        public TestStack.White.UIItems.WindowItems.Window qbWindow = null;
        public Thread ExceptionHandler = null;
        public static String startupPath = System.IO.Path.GetFullPath("..\\..\\..\\");
        public static Property conf = new Property(startupPath + "\\QBAutomation.properties");
        public string exe = conf.get("QBExePath");
        public string qbLoginUserName = conf.get("QBLoginUserName");
        public string qbLoginPassword = conf.get("QBLoginPassword");
        public static string companyFilePath = null;
        public static string companyFileName = null;
        public Random rand = new Random();
        public string testName = "OpenQbBackupFile";
        public string moduleName = "BATS";
        public string exception = "Null";
        public string category = "Null";
        public static string TestDataSourceDirectory = conf.get("TestDataSourceDirectory");
        public static string TestDataLocalDirectory = conf.get("TestDataLocalDirectory");


        [Given(StepTitle = "All the test company files are successfully copied from the source location")]
        public void CopyFiles()
        {
            FileOperations.DeleteCompanyFileInDirectory(TestDataLocalDirectory, companyFileName);
            FileOperations.CopyCompanyFileToDirectory(TestDataSourceDirectory, TestDataLocalDirectory, companyFileName);
        }

        [AndGiven(StepTitle = "Given - QuickBooks App and Window instances are available")]
        public void Setup()
        {
            qbApp = FrameworkLibraries.AppLibs.QBDT.WhiteAPI.QuickBooks.Initialize(exe);
            qbWindow = FrameworkLibraries.AppLibs.QBDT.WhiteAPI.QuickBooks.PrepareBaseState(qbApp);
            QuickBooks.ResetQBWindows(qbApp, qbWindow);
        }


        [Then(StepTitle = "Then - A QB backup company file should be opened or upgraded successfully")]
        public void OpenBackupCompanyFile()
        {
            QuickBooks.OpenOrUpgradeCompanyFile(companyFilePath, qbApp, qbWindow, true, false);
            var expectedTitleOfNewCompany = FrameworkLibraries.Utils.StringFunctions.RemoveExtentionFromFileName(companyFileName);
            Assert.Contains(expectedTitleOfNewCompany.ToUpper(), qbWindow.Title.ToUpper());
        }

        [AndThen(StepTitle = "AndThen - Perform tear down activities to ensure that there are no on-screen exceptions")]
        public void TearDown()
        {
            //QuickBooks.ResetQBWindows(qbApp, qbWindow);
        }

        [Theory]
        [Category("P1")]
        [PropertyData("TestData", PropertyType = typeof(OpenBackupFileTestDataSource))]
        public void RunOpenBackupCompanyFileTest(string fileName)
        {
            companyFileName = fileName;
            companyFilePath = TestDataLocalDirectory + fileName;
            companyFilePath = companyFilePath.Replace("\\\\", "\\");
            this.BDDfy();
        }

    }
}
