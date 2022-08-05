﻿#if USE_UNI_LUA
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
    public class CfgTablesWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(Cfg.Tables);
			Utils.BeginObjectRegister(type, L, translator, 0, 1, 4, 0);
			
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "TranslateText", _m_TranslateText);
			
			
			Utils.RegisterFunc(L, Utils.GETTER_IDX, "TbItem", _g_get_TbItem);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "TB_RoleBaseAttr", _g_get_TB_RoleBaseAttr);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "TB_SceneConfig", _g_get_TB_SceneConfig);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "TB_Scripts", _g_get_TB_Scripts);
            
			
			
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
					
					var gen_ret = new Cfg.Tables(_loader);
					translator.Push(L, gen_ret);
                    
					return 1;
				}
				
			}
			catch(System.Exception gen_e) {
				return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
			}
            return LuaAPI.luaL_error(L, "invalid arguments to Cfg.Tables constructor!");
            
        }
        
		
        
		
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_TranslateText(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Cfg.Tables gen_to_be_invoked = (Cfg.Tables)translator.FastGetCSObj(L, 1);
            
            
                
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
        static int _g_get_TbItem(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Cfg.Tables gen_to_be_invoked = (Cfg.Tables)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.TbItem);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_TB_RoleBaseAttr(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Cfg.Tables gen_to_be_invoked = (Cfg.Tables)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.TB_RoleBaseAttr);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_TB_SceneConfig(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Cfg.Tables gen_to_be_invoked = (Cfg.Tables)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.TB_SceneConfig);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_TB_Scripts(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Cfg.Tables gen_to_be_invoked = (Cfg.Tables)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.TB_Scripts);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        
        
		
		
		
		
    }
}
