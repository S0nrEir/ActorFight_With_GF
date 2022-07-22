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
    public class cfgcommonGlobalConfigWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(cfg.common.GlobalConfig);
			Utils.BeginObjectRegister(type, L, translator, 0, 4, 22, 0);
			
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetTypeId", _m_GetTypeId);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "Resolve", _m_Resolve);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "TranslateText", _m_TranslateText);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "ToString", _m_ToString);
			
			
			Utils.RegisterFunc(L, Utils.GETTER_IDX, "BagCapacity", _g_get_BagCapacity);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "BagCapacitySpecial", _g_get_BagCapacitySpecial);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "BagTempExpendableCapacity", _g_get_BagTempExpendableCapacity);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "BagTempToolCapacity", _g_get_BagTempToolCapacity);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "BagInitCapacity", _g_get_BagInitCapacity);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "QuickBagCapacity", _g_get_QuickBagCapacity);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "ClothBagCapacity", _g_get_ClothBagCapacity);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "ClothBagInitCapacity", _g_get_ClothBagInitCapacity);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "ClothBagCapacitySpecial", _g_get_ClothBagCapacitySpecial);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "BagInitItemsDropId", _g_get_BagInitItemsDropId);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "BagInitItemsDropId_Ref", _g_get_BagInitItemsDropId_Ref);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "MailBoxCapacity", _g_get_MailBoxCapacity);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "DamageParamC", _g_get_DamageParamC);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "DamageParamE", _g_get_DamageParamE);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "DamageParamF", _g_get_DamageParamF);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "DamageParamD", _g_get_DamageParamD);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "RoleSpeed", _g_get_RoleSpeed);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "MonsterSpeed", _g_get_MonsterSpeed);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "InitEnergy", _g_get_InitEnergy);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "InitViality", _g_get_InitViality);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "MaxViality", _g_get_MaxViality);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "PerVialityRecoveryTime", _g_get_PerVialityRecoveryTime);
            
			
			
			Utils.EndObjectRegister(type, L, translator, null, null,
			    null, null, null);

		    Utils.BeginClassRegister(type, L, __CreateInstance, 3, 0, 0);
			Utils.RegisterFunc(L, Utils.CLS_IDX, "DeserializeGlobalConfig", _m_DeserializeGlobalConfig_xlua_st_);
            
			
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "__ID__", cfg.common.GlobalConfig.__ID__);
            
			
			
			
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
					
					var gen_ret = new cfg.common.GlobalConfig(__buf);
					translator.Push(L, gen_ret);
                    
					return 1;
				}
				
			}
			catch(System.Exception gen_e) {
				return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
			}
            return LuaAPI.luaL_error(L, "invalid arguments to cfg.common.GlobalConfig constructor!");
            
        }
        
		
        
		
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_DeserializeGlobalConfig_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    Bright.Serialization.ByteBuf __buf = (Bright.Serialization.ByteBuf)translator.GetObject(L, 1, typeof(Bright.Serialization.ByteBuf));
                    
                        var gen_ret = cfg.common.GlobalConfig.DeserializeGlobalConfig( __buf );
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
            
            
                cfg.common.GlobalConfig gen_to_be_invoked = (cfg.common.GlobalConfig)translator.FastGetCSObj(L, 1);
            
            
                
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
            
            
                cfg.common.GlobalConfig gen_to_be_invoked = (cfg.common.GlobalConfig)translator.FastGetCSObj(L, 1);
            
            
                
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
            
            
                cfg.common.GlobalConfig gen_to_be_invoked = (cfg.common.GlobalConfig)translator.FastGetCSObj(L, 1);
            
            
                
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
            
            
                cfg.common.GlobalConfig gen_to_be_invoked = (cfg.common.GlobalConfig)translator.FastGetCSObj(L, 1);
            
            
                
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
        static int _g_get_BagCapacity(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                cfg.common.GlobalConfig gen_to_be_invoked = (cfg.common.GlobalConfig)translator.FastGetCSObj(L, 1);
                LuaAPI.xlua_pushinteger(L, gen_to_be_invoked.BagCapacity);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_BagCapacitySpecial(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                cfg.common.GlobalConfig gen_to_be_invoked = (cfg.common.GlobalConfig)translator.FastGetCSObj(L, 1);
                LuaAPI.xlua_pushinteger(L, gen_to_be_invoked.BagCapacitySpecial);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_BagTempExpendableCapacity(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                cfg.common.GlobalConfig gen_to_be_invoked = (cfg.common.GlobalConfig)translator.FastGetCSObj(L, 1);
                LuaAPI.xlua_pushinteger(L, gen_to_be_invoked.BagTempExpendableCapacity);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_BagTempToolCapacity(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                cfg.common.GlobalConfig gen_to_be_invoked = (cfg.common.GlobalConfig)translator.FastGetCSObj(L, 1);
                LuaAPI.xlua_pushinteger(L, gen_to_be_invoked.BagTempToolCapacity);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_BagInitCapacity(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                cfg.common.GlobalConfig gen_to_be_invoked = (cfg.common.GlobalConfig)translator.FastGetCSObj(L, 1);
                LuaAPI.xlua_pushinteger(L, gen_to_be_invoked.BagInitCapacity);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_QuickBagCapacity(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                cfg.common.GlobalConfig gen_to_be_invoked = (cfg.common.GlobalConfig)translator.FastGetCSObj(L, 1);
                LuaAPI.xlua_pushinteger(L, gen_to_be_invoked.QuickBagCapacity);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_ClothBagCapacity(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                cfg.common.GlobalConfig gen_to_be_invoked = (cfg.common.GlobalConfig)translator.FastGetCSObj(L, 1);
                LuaAPI.xlua_pushinteger(L, gen_to_be_invoked.ClothBagCapacity);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_ClothBagInitCapacity(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                cfg.common.GlobalConfig gen_to_be_invoked = (cfg.common.GlobalConfig)translator.FastGetCSObj(L, 1);
                LuaAPI.xlua_pushinteger(L, gen_to_be_invoked.ClothBagInitCapacity);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_ClothBagCapacitySpecial(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                cfg.common.GlobalConfig gen_to_be_invoked = (cfg.common.GlobalConfig)translator.FastGetCSObj(L, 1);
                LuaAPI.xlua_pushinteger(L, gen_to_be_invoked.ClothBagCapacitySpecial);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_BagInitItemsDropId(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                cfg.common.GlobalConfig gen_to_be_invoked = (cfg.common.GlobalConfig)translator.FastGetCSObj(L, 1);
                translator.PushAny(L, gen_to_be_invoked.BagInitItemsDropId);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_BagInitItemsDropId_Ref(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                cfg.common.GlobalConfig gen_to_be_invoked = (cfg.common.GlobalConfig)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.BagInitItemsDropId_Ref);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_MailBoxCapacity(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                cfg.common.GlobalConfig gen_to_be_invoked = (cfg.common.GlobalConfig)translator.FastGetCSObj(L, 1);
                LuaAPI.xlua_pushinteger(L, gen_to_be_invoked.MailBoxCapacity);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_DamageParamC(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                cfg.common.GlobalConfig gen_to_be_invoked = (cfg.common.GlobalConfig)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushnumber(L, gen_to_be_invoked.DamageParamC);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_DamageParamE(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                cfg.common.GlobalConfig gen_to_be_invoked = (cfg.common.GlobalConfig)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushnumber(L, gen_to_be_invoked.DamageParamE);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_DamageParamF(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                cfg.common.GlobalConfig gen_to_be_invoked = (cfg.common.GlobalConfig)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushnumber(L, gen_to_be_invoked.DamageParamF);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_DamageParamD(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                cfg.common.GlobalConfig gen_to_be_invoked = (cfg.common.GlobalConfig)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushnumber(L, gen_to_be_invoked.DamageParamD);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_RoleSpeed(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                cfg.common.GlobalConfig gen_to_be_invoked = (cfg.common.GlobalConfig)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushnumber(L, gen_to_be_invoked.RoleSpeed);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_MonsterSpeed(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                cfg.common.GlobalConfig gen_to_be_invoked = (cfg.common.GlobalConfig)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushnumber(L, gen_to_be_invoked.MonsterSpeed);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_InitEnergy(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                cfg.common.GlobalConfig gen_to_be_invoked = (cfg.common.GlobalConfig)translator.FastGetCSObj(L, 1);
                LuaAPI.xlua_pushinteger(L, gen_to_be_invoked.InitEnergy);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_InitViality(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                cfg.common.GlobalConfig gen_to_be_invoked = (cfg.common.GlobalConfig)translator.FastGetCSObj(L, 1);
                LuaAPI.xlua_pushinteger(L, gen_to_be_invoked.InitViality);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_MaxViality(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                cfg.common.GlobalConfig gen_to_be_invoked = (cfg.common.GlobalConfig)translator.FastGetCSObj(L, 1);
                LuaAPI.xlua_pushinteger(L, gen_to_be_invoked.MaxViality);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_PerVialityRecoveryTime(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                cfg.common.GlobalConfig gen_to_be_invoked = (cfg.common.GlobalConfig)translator.FastGetCSObj(L, 1);
                LuaAPI.xlua_pushinteger(L, gen_to_be_invoked.PerVialityRecoveryTime);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        
        
		
		
		
		
    }
}
