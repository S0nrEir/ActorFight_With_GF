#if USE_UNI_LUA
using LuaAPI = UniLua.Lua;
using RealStatePtr = UniLua.ILuaState;
using LuaCSFunction = UniLua.CSharpFunctionDelegate;
#else
using LuaAPI = XLua.LuaDLL.Lua;
using RealStatePtr = System.IntPtr;
using LuaCSFunction = XLua.LuaDLL.lua_CSFunction;
#endif

using XLua;
using System.Collections.Generic;


namespace XLua.CSObjectWrap
{
    using Utils = XLua.Utils;
    public class cfgTablesWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(cfg.Tables);
			Utils.BeginObjectRegister(type, L, translator, 0, 1, 57, 0);
			
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "TranslateText", _m_TranslateText);
			
			
			Utils.RegisterFunc(L, Utils.GETTER_IDX, "TbBlackboard", _g_get_TbBlackboard);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "TbBehaviorTree", _g_get_TbBehaviorTree);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "TbClazz", _g_get_TbClazz);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "TbDrop", _g_get_TbDrop);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "TbGlobalConfig", _g_get_TbGlobalConfig);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "TbErrorInfo", _g_get_TbErrorInfo);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "TbCodeInfo", _g_get_TbCodeInfo);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "TbItem", _g_get_TbItem);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "TbItemFunc", _g_get_TbItemFunc);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "TbItemExtra", _g_get_TbItemExtra);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "TbL10NDemo", _g_get_TbL10NDemo);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "TbPatchDemo", _g_get_TbPatchDemo);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "TbSystemMail", _g_get_TbSystemMail);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "TbGlobalMail", _g_get_TbGlobalMail);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "TbRoleLevelExpAttr", _g_get_TbRoleLevelExpAttr);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "TbRoleLevelBonusCoefficient", _g_get_TbRoleLevelBonusCoefficient);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "TbTestTag", _g_get_TbTestTag);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "TbFullTypes", _g_get_TbFullTypes);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "TbSingleton", _g_get_TbSingleton);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "TbNotIndexList", _g_get_TbNotIndexList);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "TbMultiUnionIndexList", _g_get_TbMultiUnionIndexList);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "TbMultiIndexList", _g_get_TbMultiIndexList);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "TbDataFromMisc", _g_get_TbDataFromMisc);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "TbMultiRowRecord", _g_get_TbMultiRowRecord);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "TbTestMultiColumn", _g_get_TbTestMultiColumn);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "TbMultiRowTitle", _g_get_TbMultiRowTitle);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "TbTestNull", _g_get_TbTestNull);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "TbDemoPrimitive", _g_get_TbDemoPrimitive);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "TbTestString", _g_get_TbTestString);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "TbDemoGroup", _g_get_TbDemoGroup);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "TbDemoGroup_C", _g_get_TbDemoGroup_C);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "TbDemoGroup_S", _g_get_TbDemoGroup_S);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "TbDemoGroup_E", _g_get_TbDemoGroup_E);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "TbTestGlobal", _g_get_TbTestGlobal);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "TbTestBeRef", _g_get_TbTestBeRef);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "TbTestBeRef2", _g_get_TbTestBeRef2);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "TbTestRef", _g_get_TbTestRef);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "TbTestSize", _g_get_TbTestSize);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "TbTestSet", _g_get_TbTestSet);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "TbDetectCsvEncoding", _g_get_TbDetectCsvEncoding);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "TbItem2", _g_get_TbItem2);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "TbDefineFromExcel", _g_get_TbDefineFromExcel);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "TbDefineFromExcelOne", _g_get_TbDefineFromExcelOne);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "TbTestIndex", _g_get_TbTestIndex);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "TbTestMap", _g_get_TbTestMap);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "TbExcelFromJson", _g_get_TbExcelFromJson);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "TbCompositeJsonTable1", _g_get_TbCompositeJsonTable1);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "TbCompositeJsonTable2", _g_get_TbCompositeJsonTable2);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "TbCompositeJsonTable3", _g_get_TbCompositeJsonTable3);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "TbExcelFromJsonMultiRow", _g_get_TbExcelFromJsonMultiRow);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "TbTestSep", _g_get_TbTestSep);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "TbTestScriptableObject", _g_get_TbTestScriptableObject);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "TbTestExternalType", _g_get_TbTestExternalType);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "TbDemoGroupDefineFromExcel", _g_get_TbDemoGroupDefineFromExcel);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "TbDefineFromExcel2", _g_get_TbDefineFromExcel2);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "TbTestExcelBean", _g_get_TbTestExcelBean);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "TbTestDesc", _g_get_TbTestDesc);
            
			
			
			Utils.EndObjectRegister(type, L, translator, null, null,
			    null, null, null);

		    Utils.BeginClassRegister(type, L, __CreateInstance, 1, 0, 0);
			
			
            
			
			
			
			Utils.EndClassRegister(type, L, translator);
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CreateInstance(RealStatePtr L)
        {
            
			try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
				if(LuaAPI.lua_gettop(L) == 2 && translator.Assignable<System.Func<string, Bright.Serialization.ByteBuf>>(L, 2))
				{
					System.Func<string, Bright.Serialization.ByteBuf> _loader = translator.GetDelegate<System.Func<string, Bright.Serialization.ByteBuf>>(L, 2);
					
					var gen_ret = new cfg.Tables(_loader);
					translator.Push(L, gen_ret);
                    
					return 1;
				}
				
			}
			catch(System.Exception gen_e) {
				return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
			}
            return LuaAPI.luaL_error(L, "invalid arguments to cfg.Tables constructor!");
            
        }
        
		
        
		
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_TranslateText(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                cfg.Tables gen_to_be_invoked = (cfg.Tables)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    System.Func<string, string, string> _translator = translator.GetDelegate<System.Func<string, string, string>>(L, 2);
                    
                    gen_to_be_invoked.TranslateText( _translator );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_TbBlackboard(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                cfg.Tables gen_to_be_invoked = (cfg.Tables)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.TbBlackboard);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_TbBehaviorTree(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                cfg.Tables gen_to_be_invoked = (cfg.Tables)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.TbBehaviorTree);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_TbClazz(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                cfg.Tables gen_to_be_invoked = (cfg.Tables)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.TbClazz);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_TbDrop(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                cfg.Tables gen_to_be_invoked = (cfg.Tables)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.TbDrop);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_TbGlobalConfig(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                cfg.Tables gen_to_be_invoked = (cfg.Tables)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.TbGlobalConfig);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_TbErrorInfo(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                cfg.Tables gen_to_be_invoked = (cfg.Tables)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.TbErrorInfo);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_TbCodeInfo(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                cfg.Tables gen_to_be_invoked = (cfg.Tables)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.TbCodeInfo);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_TbItem(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                cfg.Tables gen_to_be_invoked = (cfg.Tables)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.TbItem);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_TbItemFunc(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                cfg.Tables gen_to_be_invoked = (cfg.Tables)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.TbItemFunc);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_TbItemExtra(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                cfg.Tables gen_to_be_invoked = (cfg.Tables)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.TbItemExtra);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_TbL10NDemo(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                cfg.Tables gen_to_be_invoked = (cfg.Tables)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.TbL10NDemo);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_TbPatchDemo(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                cfg.Tables gen_to_be_invoked = (cfg.Tables)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.TbPatchDemo);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_TbSystemMail(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                cfg.Tables gen_to_be_invoked = (cfg.Tables)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.TbSystemMail);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_TbGlobalMail(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                cfg.Tables gen_to_be_invoked = (cfg.Tables)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.TbGlobalMail);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_TbRoleLevelExpAttr(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                cfg.Tables gen_to_be_invoked = (cfg.Tables)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.TbRoleLevelExpAttr);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_TbRoleLevelBonusCoefficient(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                cfg.Tables gen_to_be_invoked = (cfg.Tables)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.TbRoleLevelBonusCoefficient);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_TbTestTag(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                cfg.Tables gen_to_be_invoked = (cfg.Tables)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.TbTestTag);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_TbFullTypes(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                cfg.Tables gen_to_be_invoked = (cfg.Tables)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.TbFullTypes);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_TbSingleton(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                cfg.Tables gen_to_be_invoked = (cfg.Tables)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.TbSingleton);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_TbNotIndexList(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                cfg.Tables gen_to_be_invoked = (cfg.Tables)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.TbNotIndexList);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_TbMultiUnionIndexList(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                cfg.Tables gen_to_be_invoked = (cfg.Tables)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.TbMultiUnionIndexList);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_TbMultiIndexList(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                cfg.Tables gen_to_be_invoked = (cfg.Tables)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.TbMultiIndexList);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_TbDataFromMisc(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                cfg.Tables gen_to_be_invoked = (cfg.Tables)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.TbDataFromMisc);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_TbMultiRowRecord(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                cfg.Tables gen_to_be_invoked = (cfg.Tables)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.TbMultiRowRecord);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_TbTestMultiColumn(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                cfg.Tables gen_to_be_invoked = (cfg.Tables)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.TbTestMultiColumn);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_TbMultiRowTitle(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                cfg.Tables gen_to_be_invoked = (cfg.Tables)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.TbMultiRowTitle);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_TbTestNull(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                cfg.Tables gen_to_be_invoked = (cfg.Tables)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.TbTestNull);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_TbDemoPrimitive(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                cfg.Tables gen_to_be_invoked = (cfg.Tables)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.TbDemoPrimitive);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_TbTestString(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                cfg.Tables gen_to_be_invoked = (cfg.Tables)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.TbTestString);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_TbDemoGroup(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                cfg.Tables gen_to_be_invoked = (cfg.Tables)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.TbDemoGroup);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_TbDemoGroup_C(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                cfg.Tables gen_to_be_invoked = (cfg.Tables)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.TbDemoGroup_C);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_TbDemoGroup_S(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                cfg.Tables gen_to_be_invoked = (cfg.Tables)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.TbDemoGroup_S);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_TbDemoGroup_E(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                cfg.Tables gen_to_be_invoked = (cfg.Tables)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.TbDemoGroup_E);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_TbTestGlobal(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                cfg.Tables gen_to_be_invoked = (cfg.Tables)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.TbTestGlobal);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_TbTestBeRef(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                cfg.Tables gen_to_be_invoked = (cfg.Tables)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.TbTestBeRef);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_TbTestBeRef2(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                cfg.Tables gen_to_be_invoked = (cfg.Tables)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.TbTestBeRef2);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_TbTestRef(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                cfg.Tables gen_to_be_invoked = (cfg.Tables)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.TbTestRef);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_TbTestSize(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                cfg.Tables gen_to_be_invoked = (cfg.Tables)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.TbTestSize);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_TbTestSet(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                cfg.Tables gen_to_be_invoked = (cfg.Tables)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.TbTestSet);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_TbDetectCsvEncoding(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                cfg.Tables gen_to_be_invoked = (cfg.Tables)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.TbDetectCsvEncoding);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_TbItem2(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                cfg.Tables gen_to_be_invoked = (cfg.Tables)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.TbItem2);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_TbDefineFromExcel(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                cfg.Tables gen_to_be_invoked = (cfg.Tables)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.TbDefineFromExcel);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_TbDefineFromExcelOne(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                cfg.Tables gen_to_be_invoked = (cfg.Tables)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.TbDefineFromExcelOne);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_TbTestIndex(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                cfg.Tables gen_to_be_invoked = (cfg.Tables)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.TbTestIndex);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_TbTestMap(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                cfg.Tables gen_to_be_invoked = (cfg.Tables)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.TbTestMap);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_TbExcelFromJson(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                cfg.Tables gen_to_be_invoked = (cfg.Tables)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.TbExcelFromJson);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_TbCompositeJsonTable1(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                cfg.Tables gen_to_be_invoked = (cfg.Tables)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.TbCompositeJsonTable1);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_TbCompositeJsonTable2(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                cfg.Tables gen_to_be_invoked = (cfg.Tables)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.TbCompositeJsonTable2);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_TbCompositeJsonTable3(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                cfg.Tables gen_to_be_invoked = (cfg.Tables)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.TbCompositeJsonTable3);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_TbExcelFromJsonMultiRow(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                cfg.Tables gen_to_be_invoked = (cfg.Tables)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.TbExcelFromJsonMultiRow);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_TbTestSep(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                cfg.Tables gen_to_be_invoked = (cfg.Tables)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.TbTestSep);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_TbTestScriptableObject(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                cfg.Tables gen_to_be_invoked = (cfg.Tables)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.TbTestScriptableObject);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_TbTestExternalType(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                cfg.Tables gen_to_be_invoked = (cfg.Tables)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.TbTestExternalType);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_TbDemoGroupDefineFromExcel(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                cfg.Tables gen_to_be_invoked = (cfg.Tables)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.TbDemoGroupDefineFromExcel);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_TbDefineFromExcel2(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                cfg.Tables gen_to_be_invoked = (cfg.Tables)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.TbDefineFromExcel2);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_TbTestExcelBean(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                cfg.Tables gen_to_be_invoked = (cfg.Tables)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.TbTestExcelBean);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_TbTestDesc(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                cfg.Tables gen_to_be_invoked = (cfg.Tables)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.TbTestDesc);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        
        
		
		
		
		
    }
}
