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
    public class AquilaExtensionComponent_TimerWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(Aquila.Extension.Component_Timer);
			Utils.BeginObjectRegister(type, L, translator, 0, 4, 0, 0);
			
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "StartTick", _m_StartTick);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "StartCounting", _m_StartCounting);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "Regiseter", _m_Regiseter);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "UnRegisterUpdate", _m_UnRegisterUpdate);
			
			
			
			
			
			Utils.EndObjectRegister(type, L, translator, null, null,
			    null, null, null);

		    Utils.BeginClassRegister(type, L, __CreateInstance, 2, 0, 0);
			
			
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "MAX_TIMER_COUNT", Aquila.Extension.Component_Timer.MAX_TIMER_COUNT);
            
			
			
			
			Utils.EndClassRegister(type, L, translator);
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CreateInstance(RealStatePtr L)
        {
            
			try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
				if(LuaAPI.lua_gettop(L) == 1)
				{
					
					var gen_ret = new Aquila.Extension.Component_Timer();
					translator.Push(L, gen_ret);
                    
					return 1;
				}
				
			}
			catch(System.Exception gen_e) {
				return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
			}
            return LuaAPI.luaL_error(L, "invalid arguments to Aquila.Extension.Component_Timer constructor!");
            
        }
        
		
        
		
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_StartTick(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Aquila.Extension.Component_Timer gen_to_be_invoked = (Aquila.Extension.Component_Timer)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    float _n = (float)LuaAPI.lua_tonumber(L, 2);
                    System.Action<float> _callBack = translator.GetDelegate<System.Action<float>>(L, 3);
                    
                        var gen_ret = gen_to_be_invoked.StartTick( _n, _callBack );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_StartCounting(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Aquila.Extension.Component_Timer gen_to_be_invoked = (Aquila.Extension.Component_Timer)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    float _n = (float)LuaAPI.lua_tonumber(L, 2);
                    System.Action<float> _callBack = translator.GetDelegate<System.Action<float>>(L, 3);
                    
                        var gen_ret = gen_to_be_invoked.StartCounting( _n, _callBack );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Regiseter(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Aquila.Extension.Component_Timer gen_to_be_invoked = (Aquila.Extension.Component_Timer)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    Aquila.Extension.Component_Timer.Timer _timer = (Aquila.Extension.Component_Timer.Timer)translator.GetObject(L, 2, typeof(Aquila.Extension.Component_Timer.Timer));
                    System.Collections.Generic.Dictionary<int, Aquila.Extension.Component_Timer.Timer> _dicToRegister = (System.Collections.Generic.Dictionary<int, Aquila.Extension.Component_Timer.Timer>)translator.GetObject(L, 3, typeof(System.Collections.Generic.Dictionary<int, Aquila.Extension.Component_Timer.Timer>));
                    
                    gen_to_be_invoked.Regiseter( _timer, _dicToRegister );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_UnRegisterUpdate(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Aquila.Extension.Component_Timer gen_to_be_invoked = (Aquila.Extension.Component_Timer)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    Aquila.Extension.Component_Timer.Timer _timer = (Aquila.Extension.Component_Timer.Timer)translator.GetObject(L, 2, typeof(Aquila.Extension.Component_Timer.Timer));
                    
                    gen_to_be_invoked.UnRegisterUpdate( _timer );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        
        
        
        
        
		
		
		
		
    }
}
