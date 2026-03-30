using System;
using System.Diagnostics;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Editor.AbilityEditor.Tools
{
    /// <summary>
    /// Effect 验证工具的 Unity Editor 菜单入口
    /// </summary>
    public static class EffectVerificationMenu
    {
        private const string TEMP_EXPORT_PATH = "Temp/EffectVerification";
        private const string LOG_PATH = "Log/Effect/effect_verification.txt";

        [MenuItem("Aquila/AbilityEditor/Tools/Verify Effect Export", false, 210)]
        public static void VerifyEffectExport()
        {
            if (!EditorUtility.DisplayDialog(
                "Verify Effect Export",
                "This will export all effects to temporary files and verify their correctness.\n\n" +
                "The process may take a few moments.",
                "Start Verification",
                "Cancel"))
            {
                return;
            }

            // 获取绝对路径
            string projectPath = Path.GetDirectoryName(Application.dataPath);
            string tempPath = Path.Combine(projectPath, TEMP_EXPORT_PATH);
            string logPath = Path.Combine(projectPath, LOG_PATH);

            try
            {
                EditorUtility.DisplayProgressBar("Effect Verification", "Starting verification...", 0f);

                // 执行验证
                var result = EffectVerificationTool.VerifyAllEffects(tempPath, logPath);

                EditorUtility.ClearProgressBar();

                // 显示结果
                ShowResult(result, logPath);
            }
            catch (Exception ex)
            {
                EditorUtility.ClearProgressBar();
                EditorUtility.DisplayDialog(
                    "Verification Error",
                    $"An error occurred during verification:\n\n{ex.Message}",
                    "OK");
                Aquila.Toolkit.Tools.Logger.Error($"[EffectVerificationMenu] Verification failed: {ex.Message}\n{ex.StackTrace}");
            }
        }

        private static void ShowResult(EffectVerificationTool.VerificationResult result, string logPath)
        {
            if (result.IsSuccess)
            {
                EditorUtility.DisplayDialog(
                    "Verification Successful",
                    $"All {result.SuccessfulEffects.Count} effects verified successfully!\n\n" +
                    "All exported binary files match the original configurations.",
                    "OK");
            }
            else
            {
                string message = "Verification completed with issues:\n\n" +
                                $"Total Effects: {result.TotalEffects}\n" +
                                $"Successful: {result.SuccessfulEffects.Count}\n" +
                                $"Failed: {result.Failures.Count}\n\n";

                if (!string.IsNullOrEmpty(result.ErrorMessage))
                {
                    message += $"Error: {result.ErrorMessage}\n\n";
                }

                if (result.Failures.Count > 0)
                {
                    message += "Failed Effect IDs:\n";
                    foreach (var failure in result.Failures)
                    {
                        message += $"  - {failure.EffectId}";
                        if (!string.IsNullOrEmpty(failure.ErrorMessage))
                        {
                            message += $" ({failure.ErrorMessage})";
                        }
                        message += "\n";
                    }
                    message += "\n";
                }

                message += $"Detailed report saved to:\n{logPath}\n\n" +
                          "Would you like to open the log file?";

                if (EditorUtility.DisplayDialog("Verification Failed", message, "Open Log", "Close"))
                {
                    OpenLogFile(logPath);
                }
            }
        }

        private static void OpenLogFile(string logPath)
        {
            if (File.Exists(logPath))
            {
                Process.Start(logPath);
            }
            else
            {
                EditorUtility.DisplayDialog(
                    "Log File Not Found",
                    $"Could not find log file at:\n{logPath}",
                    "OK");
            }
        }

        [MenuItem("Aquila/AbilityEditor/Tools/Open Effect Verification Log", false, 211)]
        public static void OpenVerificationLog()
        {
            string projectPath = Path.GetDirectoryName(Application.dataPath);
            string logPath = Path.Combine(projectPath, LOG_PATH);

            if (File.Exists(logPath))
            {
                Process.Start(logPath);
            }
            else
            {
                EditorUtility.DisplayDialog(
                    "Log File Not Found",
                    $"No verification log found at:\n{logPath}\n\n" +
                    "Please run the verification tool first.",
                    "OK");
            }
        }

        [MenuItem("Aquila/AbilityEditor/Tools/Open Effect Verification Log", true)]
        public static bool ValidateOpenVerificationLog()
        {
            string projectPath = Path.GetDirectoryName(Application.dataPath);
            string logPath = Path.Combine(projectPath, LOG_PATH);
            return File.Exists(logPath);
        }
    }
}
