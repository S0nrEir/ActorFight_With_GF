using System;
using System.Diagnostics;
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
                "This will:\n" +
                "1. Export all abilities to temporary files and verify binary round-trip.\n" +
                "2. Read production .ablt/.efct files, assemble AbilityData, and cross-verify.\n\n" +
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
                EditorUtility.DisplayProgressBar("Ability Verification", "Phase 1: Export round-trip verification...", 0f);
                var exportResult = AbilityVerificationTool.VerifyAllAbilities(tempPath, logPath);
                EditorUtility.DisplayProgressBar("Ability Verification", "Phase 2: AbilityData assembly verification...", 0.5f);
                var assemblyResult = AbilityDataAssemblyVerifier.VerifyAssembly(logPath);
                EditorUtility.ClearProgressBar();
                ShowResult(exportResult, assemblyResult, logPath);
            }
            catch (Exception ex)
            {
                EditorUtility.ClearProgressBar();
                EditorUtility.DisplayDialog(
                    "Verification Error",
                    $"An error occurred during verification:\n\n{ex.Message}",
                    "OK");
                Aquila.Toolkit.Tools.Logger.Error($"[AbilityVerificationMenu] Verification failed: {ex.Message}\n{ex.StackTrace}");
            }
        }

        private static void ShowResult(
            AbilityVerificationTool.VerificationResult exportResult,
            AbilityDataAssemblyVerifier.AssemblyVerificationResult assemblyResult,
            string logPath)
        {
            bool allSuccess = exportResult.IsSuccess && assemblyResult.IsSuccess;

            if (allSuccess)
            {
                EditorUtility.DisplayDialog(
                    "Verification Successful",
                    $"Phase 1 - Export Round-Trip: All {exportResult.SuccessfulAbilities.Count} abilities verified.\n" +
                    $"Phase 2 - Assembly: All {assemblyResult.SuccessfulAbilities.Count} abilities assembled and cross-verified.\n\n" +
                    "All data is consistent.",
                    "OK");
            }
            else
            {
                string message = "=== Phase 1: Export Round-Trip ===\n" +
                                $"Total: {exportResult.TotalAbilities} | " +
                                $"OK: {exportResult.SuccessfulAbilities.Count} | " +
                                $"Failed: {exportResult.Failures.Count}\n";

                if (!string.IsNullOrEmpty(exportResult.ErrorMessage))
                    message += $"Error: {exportResult.ErrorMessage}\n";

                if (exportResult.Failures.Count > 0)
                {
                    foreach (var failure in exportResult.Failures)
                    {
                        message += $"  - Ability {failure.AbilityId}";
                        if (!string.IsNullOrEmpty(failure.ErrorMessage))
                            message += $" ({failure.ErrorMessage})";
                        message += "\n";
                    }
                }

                message += "\n=== Phase 2: Assembly Verification ===\n" +
                          $"Total: {assemblyResult.TotalAbilities} | " +
                          $"OK: {assemblyResult.SuccessfulAbilities.Count} | " +
                          $"Failed: {assemblyResult.Failures.Count}\n";

                if (!string.IsNullOrEmpty(assemblyResult.ErrorMessage))
                    message += $"Error: {assemblyResult.ErrorMessage}\n";

                if (assemblyResult.Failures.Count > 0)
                {
                    foreach (var failure in assemblyResult.Failures)
                    {
                        message += $"  - Ability {failure.AbilityId}";
                        if (!string.IsNullOrEmpty(failure.ErrorMessage))
                            message += $" ({failure.ErrorMessage})";
                        message += "\n";
                    }
                }

                message += $"\nDetailed report: {logPath}\nOpen log file?";

                if (EditorUtility.DisplayDialog("Verification Results", message, "Open Log", "Close"))
                    OpenLogFile(logPath);
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
                Process.Start(logPath);
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
