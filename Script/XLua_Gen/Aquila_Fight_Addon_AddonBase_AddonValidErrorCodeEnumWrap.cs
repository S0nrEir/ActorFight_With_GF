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
    public class AquilaFightAddonAddonBaseAddonValidErrorCodeEnumWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(Aquila.Fight.Addon.AddonBase.AddonValidErrorCodeEnum);
			Utils.BeginObjectRegister(type, L, translator, 0, 0, 0, 0);
			
			
			
			
			
			
			Utils.EndObjectRegister(type, L, translator, null, null,
			    null, null, null);

		    Utils.BeginClassRegister(type, L, __CreateInstance, 5, 0, 0);
			Utils.RegisterFunc(L, Utils.CLS_IDX, "ErrCode2String", _m_ErrCode2String_xlua_st_);
            
			
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "NONE", Aquila.Fight.Addon.AddonBase.AddonValidErrorCodeEnum.NONE);
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "ZERO_DATA_COUNT", Aquila.Fight.Addon.AddonBase.AddonValidErrorCodeEnum.ZERO_DATA_COUNT);
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "NONE_EVENT", Aquila.Fight.Addon.AddonBase.AddonValidErrorCodeEnum.NONE_EVENT);
            
			
			
			
			Utils.EndClassRegister(type, L, translator);
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CreateInstance(RealStatePtr L)
        {
            
			try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
				if(LuaAPI.lua_gettop(L) == 1)
				{
					
					var gen_ret = new Aquila.Fight.Addon.AddonBase.AddonValidErrorCodeEnum();
					translator.Push(L, gen_ret);
                    
					return 1;
				}
				
			}
			catch(System.Exception gen_e) {
				return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
			}
            return LuaAPI.luaL_error(L, "invalid arguments to Aquila.Fight.Addon.AddonBase.AddonValidErrorCodeEnum constructor!");
            
        }
        
		
        
		
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ErrCode2String_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
                
                {
                    uint _errCode = LuaAPI.xlua_touint(L, 1);
                    
                        var gen_ret = Aquila.Fight.Addon.AddonBase.AddonValidErrorCodeEnum.ErrCode2String( _errCode );
                        LuaAPI.lua_pushstring(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        
        
        
        
        
		
		
		
		
    }
}
