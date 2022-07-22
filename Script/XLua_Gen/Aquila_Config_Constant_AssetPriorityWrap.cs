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
    public class AquilaConfigConstantAssetPriorityWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(Aquila.Config.Constant.AssetPriority);
			Utils.BeginObjectRegister(type, L, translator, 0, 0, 0, 0);
			
			
			
			
			
			
			Utils.EndObjectRegister(type, L, translator, null, null,
			    null, null, null);

		    Utils.BeginClassRegister(type, L, __CreateInstance, 18, 0, 0);
			
			
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "ConfigAsset", Aquila.Config.Constant.AssetPriority.ConfigAsset);
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "DataTableAsset", Aquila.Config.Constant.AssetPriority.DataTableAsset);
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "DictionaryAsset", Aquila.Config.Constant.AssetPriority.DictionaryAsset);
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "FontAsset", Aquila.Config.Constant.AssetPriority.FontAsset);
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "MusicAsset", Aquila.Config.Constant.AssetPriority.MusicAsset);
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "SceneAsset", Aquila.Config.Constant.AssetPriority.SceneAsset);
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "SoundAsset", Aquila.Config.Constant.AssetPriority.SoundAsset);
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "UIFormAsset", Aquila.Config.Constant.AssetPriority.UIFormAsset);
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "UISoundAsset", Aquila.Config.Constant.AssetPriority.UISoundAsset);
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "MyAircraftAsset", Aquila.Config.Constant.AssetPriority.MyAircraftAsset);
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "AircraftAsset", Aquila.Config.Constant.AssetPriority.AircraftAsset);
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "ThrusterAsset", Aquila.Config.Constant.AssetPriority.ThrusterAsset);
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "WeaponAsset", Aquila.Config.Constant.AssetPriority.WeaponAsset);
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "ArmorAsset", Aquila.Config.Constant.AssetPriority.ArmorAsset);
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "BulletAsset", Aquila.Config.Constant.AssetPriority.BulletAsset);
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "AsteroiAsset", Aquila.Config.Constant.AssetPriority.AsteroiAsset);
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "EffectAsset", Aquila.Config.Constant.AssetPriority.EffectAsset);
            
			
			
			
			Utils.EndClassRegister(type, L, translator);
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CreateInstance(RealStatePtr L)
        {
            return LuaAPI.luaL_error(L, "Aquila.Config.Constant.AssetPriority does not have a constructor!");
        }
        
		
        
		
        
        
        
        
        
        
        
        
        
		
		
		
		
    }
}
