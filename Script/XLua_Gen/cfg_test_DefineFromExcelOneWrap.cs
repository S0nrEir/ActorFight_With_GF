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
    public class cfgtestDefineFromExcelOneWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(cfg.test.DefineFromExcelOne);
			Utils.BeginObjectRegister(type, L, translator, 0, 4, 6, 0);
			
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetTypeId", _m_GetTypeId);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "Resolve", _m_Resolve);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "TranslateText", _m_TranslateText);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "ToString", _m_ToString);
			
			
			Utils.RegisterFunc(L, Utils.GETTER_IDX, "UnlockEquip", _g_get_UnlockEquip);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "UnlockHero", _g_get_UnlockHero);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "DefaultAvatar", _g_get_DefaultAvatar);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "DefaultItem", _g_get_DefaultItem);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "E2", _g_get_E2);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "Icons", _g_get_Icons);
            
			
			
			Utils.EndObjectRegister(type, L, translator, null, null,
			    null, null, null);

		    Utils.BeginClassRegister(type, L, __CreateInstance, 3, 0, 0);
			Utils.RegisterFunc(L, Utils.CLS_IDX, "DeserializeDefineFromExcelOne", _m_DeserializeDefineFromExcelOne_xlua_st_);
            
			
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "__ID__", cfg.test.DefineFromExcelOne.__ID__);
            
			
			
			
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
					
					var gen_ret = new cfg.test.DefineFromExcelOne(__buf);
					translator.Push(L, gen_ret);
                    
					return 1;
				}
				
			}
			catch(System.Exception gen_e) {
				return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
			}
            return LuaAPI.luaL_error(L, "invalid arguments to cfg.test.DefineFromExcelOne constructor!");
            
        }
        
		
        
		
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_DeserializeDefineFromExcelOne_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    Bright.Serialization.ByteBuf __buf = (Bright.Serialization.ByteBuf)translator.GetObject(L, 1, typeof(Bright.Serialization.ByteBuf));
                    
                        var gen_ret = cfg.test.DefineFromExcelOne.DeserializeDefineFromExcelOne( __buf );
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
            
            
                cfg.test.DefineFromExcelOne gen_to_be_invoked = (cfg.test.DefineFromExcelOne)translator.FastGetCSObj(L, 1);
            
            
                
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
            
            
                cfg.test.DefineFromExcelOne gen_to_be_invoked = (cfg.test.DefineFromExcelOne)translator.FastGetCSObj(L, 1);
            
            
                
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
            
            
                cfg.test.DefineFromExcelOne gen_to_be_invoked = (cfg.test.DefineFromExcelOne)translator.FastGetCSObj(L, 1);
            
            
                
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
            
            
                cfg.test.DefineFromExcelOne gen_to_be_invoked = (cfg.test.DefineFromExcelOne)translator.FastGetCSObj(L, 1);
            
            
                
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
        static int _g_get_UnlockEquip(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                cfg.test.DefineFromExcelOne gen_to_be_invoked = (cfg.test.DefineFromExcelOne)translator.FastGetCSObj(L, 1);
                LuaAPI.xlua_pushinteger(L, gen_to_be_invoked.UnlockEquip);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_UnlockHero(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                cfg.test.DefineFromExcelOne gen_to_be_invoked = (cfg.test.DefineFromExcelOne)translator.FastGetCSObj(L, 1);
                LuaAPI.xlua_pushinteger(L, gen_to_be_invoked.UnlockHero);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_DefaultAvatar(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                cfg.test.DefineFromExcelOne gen_to_be_invoked = (cfg.test.DefineFromExcelOne)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushstring(L, gen_to_be_invoked.DefaultAvatar);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_DefaultItem(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                cfg.test.DefineFromExcelOne gen_to_be_invoked = (cfg.test.DefineFromExcelOne)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushstring(L, gen_to_be_invoked.DefaultItem);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_E2(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                cfg.test.DefineFromExcelOne gen_to_be_invoked = (cfg.test.DefineFromExcelOne)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.E2);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_Icons(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                cfg.test.DefineFromExcelOne gen_to_be_invoked = (cfg.test.DefineFromExcelOne)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.Icons);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        
        
		
		
		
		
    }
}
