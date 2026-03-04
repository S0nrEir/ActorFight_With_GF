using System.IO;
using UnityEditor;
using UnityEngine;

namespace Editor.AbilityEditor.Tools
{
    /// <summary>
    /// Ability 验证工具的 Unity Editor 菜单入口
    /// </summary>
    public static class AbilityVerificationMenu
    {
        [MenuItem("Aquila/AbilityEditor/Tools/Verify Ability Export", false, 200)]
        public static void VerifyAbilityExport()
        {
            if (!EditorUtility.DisplayDialog(
                "Verify Ability Export",
                "This will export all abilities to temporary files and verify their correctness.\n\n" +
                "The process may take a few moments.",
                "Start Verification",
                "Cancel"))
            {
                return;
            }

            string projectPath = Path.GetDirectoryName(Application.dataPath);
            string tempPath = Path.Combine(projectPath, TEMP_EXPORT_PATH);
            string logPath = GetLogPath();
            try
            {
                EditorUtility.DisplayProgressBar("Ability Verification", "Starting verification...", 0f);

                //验证然后显示结果 / verify then show result
                var result = AbilityVerificationTool.VerifyAllAbilities(tempPath, logPath);
                EditorUtility.ClearProgressBar();
                ShowResult(result, logPath);
            }
            catch (System.Exception ex)
            {
                EditorUtility.ClearProgressBar();
                EditorUtility.DisplayDialog(
                    "Verification Error",
                    $"An error occurred during verification:\n\n{ex.Message}",
                    "OK");
                Debug.LogError($"[AbilityVerificationMenu] Verification failed: {ex.Message}\n{ex.StackTrace}");
            }
        }

        private static void ShowResult(AbilityVerificationTool.VerificationResult result, string logPath)
        {
            if (result.IsSuccess)
            {
                EditorUtility.DisplayDialog(
                    "Verification Successful",
                    $"All {result.SuccessfulAbilities.Count} abilities verified successfully!\n\n" +
                    $"All exported binary files match the original configurations.",
                    "OK");
            }
            else
            {
                string message = $"Verification completed with issues:\n\n" +
                                $"Total Abilities: {result.TotalAbilities}\n" +
                                $"Successful: {result.SuccessfulAbilities.Count}\n" +
                                $"Failed: {result.Failures.Count}\n\n";

                if (!string.IsNullOrEmpty(result.ErrorMessage))
                    message += $"Error: {result.ErrorMessage}\n\n";

                if (result.Failures.Count > 0)
                {
                    message += "Failed Ability IDs:\n";
                    foreach (var failure in result.Failures)
                    {
                        message += $"  - {failure.AbilityId}";
                        if (!string.IsNullOrEmpty(failure.ErrorMessage))
                            message += $" ({failure.ErrorMessage})";
                        
                        message += "\n";
                    }
                    message += "\n";
                }

                message += $"Detailed report saved to:\n{logPath}\n\nWould you like to open the log file?";

                if (EditorUtility.DisplayDialog("Verification Failed", message, "Open Log", "Close"))
                    OpenLogFile(logPath);
            }
        }

        private static void OpenLogFile(string logPath)
        {
            if (File.Exists(logPath))
            {
                System.Diagnostics.Process.Start(logPath);
            }
            else
            {
                EditorUtility.DisplayDialog(
                    title:"Log File Not Found",
                    message:$"Could not find log file at:\n{logPath}",
                    ok:"OK");
            }
        }

        [MenuItem("Aquila/AbilityEditor/Tools/Open Verification Log", false, 201)]
        public static void OpenVerificationLog()
        {
            string projectPath = Path.GetDirectoryName(Application.dataPath);
            string logPath = GetLogPath();
            if (File.Exists(logPath))
            {
                System.Diagnostics.Process.Start(logPath);
            }
            else
            {
                EditorUtility.DisplayDialog(
                    title:"Log File Not Found",
                    message:$"No verification log found at:\n{logPath}\n\nPlease run the verification tool first.",
                    ok:"OK");
            }
        }

        [MenuItem("Aquila/AbilityEditor/Tools/Open Verification Log", true)]
        public static bool ValidateOpenVerificationLog()
        {
            return File.Exists(GetLogPath());
        }
        
        private static string GetLogPath()
        {
            return Path.Combine(Application.dataPath, "..", "Log", "Ability", "ability_verification.txt");
        }
        
        private const string TEMP_EXPORT_PATH = "Temp/AbilityVerification";
    }
}
