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
    public class cfgtestDefineFromExcelWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(cfg.test.DefineFromExcel);
			Utils.BeginObjectRegister(type, L, translator, 0, 4, 16, 0);
			
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetTypeId", _m_GetTypeId);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "Resolve", _m_Resolve);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "TranslateText", _m_TranslateText);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "ToString", _m_ToString);
			
			
			Utils.RegisterFunc(L, Utils.GETTER_IDX, "Id", _g_get_Id);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "X1", _g_get_X1);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "X5", _g_get_X5);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "X6", _g_get_X6);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "X8", _g_get_X8);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "X10", _g_get_X10);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "X13", _g_get_X13);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "X14", _g_get_X14);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "X15", _g_get_X15);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "V2", _g_get_V2);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "T1", _g_get_T1);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "T1_Millis", _g_get_T1_Millis);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "K1", _g_get_K1);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "K2", _g_get_K2);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "K8", _g_get_K8);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "K9", _g_get_K9);
            
			
			
			Utils.EndObjectRegister(type, L, translator, null, null,
			    null, null, null);

		    Utils.BeginClassRegister(type, L, __CreateInstance, 3, 0, 0);
			Utils.RegisterFunc(L, Utils.CLS_IDX, "DeserializeDefineFromExcel", _m_DeserializeDefineFromExcel_xlua_st_);
            
			
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "__ID__", cfg.test.DefineFromExcel.__ID__);
            
			
			
			
			Utils.EndClassRegister(type, L, translator);
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CreateInstance(RealStatePtr L)
        {
            
			try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
				if(LuaAPI.lua_gettop(L) == 2 && translator.Assignable<Bright.Serialization.ByteBuf>(L, 2))
				{
					Bright.Serialization.ByteBuf __buf = (Bright.Serialization.ByteBuf)translator.GetObject(L, 2, typeof(Bright.Serialization.ByteBuf));
					
					var gen_ret = new cfg.test.DefineFromExcel(__buf);
					translator.Push(L, gen_ret);
                    
					return 1;
				}
				
			}
			catch(System.Exception gen_e) {
				return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
			}
            return LuaAPI.luaL_error(L, "invalid arguments to cfg.test.DefineFromExcel constructor!");
            
        }
        
		
        
		
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_DeserializeDefineFromExcel_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    Bright.Serialization.ByteBuf __buf = (Bright.Serialization.ByteBuf)translator.GetObject(L, 1, typeof(Bright.Serialization.ByteBuf));
                    
                        var gen_ret = cfg.test.DefineFromExcel.DeserializeDefineFromExcel( __buf );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetTypeId(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                cfg.test.DefineFromExcel gen_to_be_invoked = (cfg.test.DefineFromExcel)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                        var gen_ret = gen_to_be_invoked.GetTypeId(  );
                        LuaAPI.xlua_pushinteger(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Resolve(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                cfg.test.DefineFromExcel gen_to_be_invoked = (cfg.test.DefineFromExcel)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    System.Collections.Generic.Dictionary<string, object> __tables = (System.Collections.Generic.Dictionary<string, object>)translator.GetObject(L, 2, typeof(System.Collections.Generic.Dictionary<string, object>));
                    
                    gen_to_be_invoked.Resolve( __tables );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_TranslateText(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                cfg.test.DefineFromExcel gen_to_be_invoked = (cfg.test.DefineFromExcel)translator.FastGetCSObj(L, 1);
            
            
                
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
        static int _m_ToString(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                cfg.test.DefineFromExcel gen_to_be_invoked = (cfg.test.DefineFromExcel)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                        var gen_ret = gen_to_be_invoked.ToString(  );
                        LuaAPI.lua_pushstring(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_Id(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                cfg.test.DefineFromExcel gen_to_be_invoked = (cfg.test.DefineFromExcel)translator.FastGetCSObj(L, 1);
                LuaAPI.xlua_pushinteger(L, gen_to_be_invoked.Id);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_X1(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                cfg.test.DefineFromExcel gen_to_be_invoked = (cfg.test.DefineFromExcel)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, gen_to_be_invoked.X1);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_X5(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                cfg.test.DefineFromExcel gen_to_be_invoked = (cfg.test.DefineFromExcel)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushint64(L, gen_to_be_invoked.X5);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_X6(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                cfg.test.DefineFromExcel gen_to_be_invoked = (cfg.test.DefineFromExcel)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushnumber(L, gen_to_be_invoked.X6);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_X8(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                cfg.test.DefineFromExcel gen_to_be_invoked = (cfg.test.DefineFromExcel)translator.FastGetCSObj(L, 1);
                LuaAPI.xlua_pushinteger(L, gen_to_be_invoked.X8);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_X10(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                cfg.test.DefineFromExcel gen_to_be_invoked = (cfg.test.DefineFromExcel)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushstring(L, gen_to_be_invoked.X10);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_X13(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                cfg.test.DefineFromExcel gen_to_be_invoked = (cfg.test.DefineFromExcel)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.X13);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_X14(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                cfg.test.DefineFromExcel gen_to_be_invoked = (cfg.test.DefineFromExcel)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.X14);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_X15(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                cfg.test.DefineFromExcel gen_to_be_invoked = (cfg.test.DefineFromExcel)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.X15);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_V2(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                cfg.test.DefineFromExcel gen_to_be_invoked = (cfg.test.DefineFromExcel)translator.FastGetCSObj(L, 1);
                translator.PushUnityEngineVector2(L, gen_to_be_invoked.V2);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_T1(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                cfg.test.DefineFromExcel gen_to_be_invoked = (cfg.test.DefineFromExcel)translator.FastGetCSObj(L, 1);
                LuaAPI.xlua_pushinteger(L, gen_to_be_invoked.T1);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_T1_Millis(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                cfg.test.DefineFromExcel gen_to_be_invoked = (cfg.test.DefineFromExcel)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushint64(L, gen_to_be_invoked.T1_Millis);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_K1(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                cfg.test.DefineFromExcel gen_to_be_invoked = (cfg.test.DefineFromExcel)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.K1);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_K2(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                cfg.test.DefineFromExcel gen_to_be_invoked = (cfg.test.DefineFromExcel)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.K2);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_K8(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                cfg.test.DefineFromExcel gen_to_be_invoked = (cfg.test.DefineFromExcel)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.K8);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_K9(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                cfg.test.DefineFromExcel gen_to_be_invoked = (cfg.test.DefineFromExcel)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.K9);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        
        
		
		
		
		
    }
}