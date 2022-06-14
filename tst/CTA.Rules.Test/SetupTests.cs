﻿using CTA.Rules.Config;
using NUnit.Framework;
using System;
using System.IO;
using System.IO.Compression;
using System.Threading;

[assembly: LevelOfParallelism(10)]
namespace CTA.Rules.Test
{
    [SetUpFixture]
    public class SetupTests : AwsRulesBaseTest
    {
        public static string TempDir;
        public static string DownloadLocation;

        [OneTimeSetUp]
        public void Setup()
        {
            Setup(this.GetType());
            TempDir = GetTstPath(Path.Combine(new string[] { "Projects", "Temp", "CTA" }));
            DownloadTestProjects();
        }
        private void DownloadTestProjects()
        {
            var tempDirectory = Directory.CreateDirectory(TempDir);
            DownloadLocation = Path.Combine(tempDirectory.FullName, "d");

            var fileName = Path.Combine(tempDirectory.Parent.FullName, @"TestProjects.zip");
            Utils.SaveFileFromGitHub(fileName, GithubInfo.TestGithubOwner, GithubInfo.TestGithubRepo, GithubInfo.TestGithubTag);
            ZipFile.ExtractToDirectory(fileName, DownloadLocation, true);
        }

        [OneTimeTearDown]
        public void Cleanup()
        {
            DeleteDir(0);
        }

        private void DeleteDir(int retries)
        {
            if (retries <= 10)
            {
                try
                {
                    Directory.Delete(TempDir, true);
                }
                catch (Exception)
                {
                    Thread.Sleep(60000);
                    DeleteDir(retries + 1);
                }
            }
        }
    }
}
