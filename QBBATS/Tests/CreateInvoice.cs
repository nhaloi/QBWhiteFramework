﻿using System;
using FrameworkLibraries.AppLibs.QBDT;
using FrameworkLibraries.Utils;
using FrameworkLibraries.ActionLibs.QBDT;
using TestStack.White.UIItems.WindowItems;
using TestStack.White.UIItems;
using TestStack.White.UIItems.Finders;
using TestStack.White;
using System.Threading;
using FrameworkLibraries.ActionLibs.QBDT.WhiteAPI;
using FrameworkLibraries.EntityFramework;
using Xunit;
using TestStack.BDDfy;
using FrameworkLibraries.AppLibs.QBDT.WhiteAPI;
using QBBATS.Data;


namespace BATS.Tests
{
    
    public class CreateInvoice
    {
        public TestStack.White.Application qbApp = null;
        public TestStack.White.UIItems.WindowItems.Window qbWindow = null;
        public static String startupPath = System.IO.Path.GetFullPath("..\\..\\..\\");
        public static Property conf = new Property("C:" + "\\QBAutomation.properties");
        public string exe = conf.get("QBExePath");
        public string qbLoginUserName = conf.get("QBLoginUserName");
        public string qbLoginPassword = conf.get("QBLoginPassword");
        public Random rand = new Random();
        public int invoiceNumber, poNumber;
        public string testName = "Invoice_POC";
        public string moduleName = "Invoice";
        public string exception = "Null";
        public string category = "Null";
        public static string TestDataSourceDirectory = conf.get("TestDataSourceDirectory");
        public static string TestDataLocalDirectory = conf.get("TestDataLocalDirectory");
        public static string filePath = startupPath + "TestData\\" + "Falcon.qbw";

        [Given(StepTitle = "Given - QuickBooks App and Window instances are available")]
        public void Setup()
        {
            qbApp = FrameworkLibraries.AppLibs.QBDT.WhiteAPI.QuickBooks.Initialize(exe);
            qbWindow = FrameworkLibraries.AppLibs.QBDT.WhiteAPI.QuickBooks.PrepareBaseState(qbApp);
            QuickBooks.ResetQBWindows(qbApp, qbWindow);
            invoiceNumber = rand.Next(12345, 99999);
            poNumber = rand.Next(12345, 99999);
        }

        [When(StepTitle = "When - A company file is opened or upgraded successfully for creating a transaction")]
        public void OpenCompanyFile()
        {
            if (!qbWindow.Title.Contains("Falcon"))
            {
                FileOperations.DeleteCompanyFileInDirectory(TestDataLocalDirectory, "Falcon");
                FileOperations.CopyCompanyFileToDirectory(TestDataSourceDirectory, TestDataLocalDirectory, "Falcon");
                FrameworkLibraries.AppLibs.QBDT.WhiteAPI.QuickBooks.OpenOrUpgradeCompanyFile(filePath, qbApp, qbWindow, false, false);
            }
        }

        [Then(StepTitle = "Then - An Invoice should be created successfully")]
        public void CreateInvoiceTest()
        {
            var customer = Invoice.Default.Customer_Job;
            var clss = Invoice.Default.Class;
            var account = Invoice.Default.Account;
            var template = Invoice.Default.Template;
            var rep = Invoice.Default.REP;
            var fob = Invoice.Default.FOB;
            var via = Invoice.Default.VIA;
            var item = Invoice.Default.Item;
            var quantity = Invoice.Default.Quantity;
            var itemDescription = "QuickBooks BATS";

            FrameworkLibraries.AppLibs.QBDT.WhiteAPI.QuickBooks.CreateInvoice(qbApp, qbWindow, customer, clss, account, template, invoiceNumber,
                poNumber, rep, via, fob, quantity, item, itemDescription, false);
        }

        [AndThen(StepTitle = "AndThen - Perform tear down activities to ensure that there are no on-screen exceptions")]
        public void TearDown()
        {
            QuickBooks.ResetQBWindows(qbApp, qbWindow);
        }

        [Fact]
        [Category("P3")]
        public void RunCreateInovoiceTest()
        {
            this.BDDfy();
        }
    }
}
