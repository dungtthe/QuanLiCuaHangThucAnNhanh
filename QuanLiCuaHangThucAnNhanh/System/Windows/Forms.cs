﻿
namespace System.Windows
{
    internal class Forms
    {
        internal class FolderBrowserDialog
        {
            public FolderBrowserDialog()
            {
            }

            public string SelectedPath { get; internal set; }
            public string Description { get; internal set; }
            public bool ShowNewFolderButton { get; internal set; }

            internal DialogResult ShowDialog()
            {
                throw new NotImplementedException();
            }
        }

        internal class DialogResult
        {
            public static DialogResult OK { get; internal set; }
        }
    }
}