using UnityEngine;
using ZLCEditor.Utils;
namespace ZLCEditor.Converter
{
    /// <summary>
    /// 中间数据工厂
    /// </summary>
    public class ILFactory : IDisposable
    {
        private Dictionary<FT, object> _converterDic;
        public ILFactory()
        {
            // 反射获取全部转换器
            _converterDic = new Dictionary<FT, object>();
            var assemblies = ConvertToolSO.Instance.assemblies;
            var childTypes = EditorAssemblyHelper.GetEnumerableChildType(assemblies, typeof(IConverter<,>));
            foreach (var childType in childTypes) {
                var instance = (IConverter)Activator.CreateInstance(childType);
                _converterDic.Add(new FT(instance.GetF(), instance.GetT()),instance);
            }
        }

        /// <summary>
        /// From，To
        /// </summary>
        private struct FT : IEquatable<FT>
        {
            private Type _from;
            private Type _to;

            internal FT(Type from, Type to)
            {
                this._from = from;
                this._to = to;
            }

            public bool Equals(FT other)
            {
                return Equals(_from, other._from) && Equals(_to, other._to);
            }
            public override bool Equals(object obj)
            {
                return obj is FT other && Equals(other);
            }
            public override int GetHashCode()
            {
                return HashCode.Combine(_from, _to);
            }
        }

        /// <summary>
        /// 获取转换器
        /// </summary>
        /// <typeparam name="F">from格式</typeparam>
        /// <typeparam name="T">to格式</typeparam>
        /// <returns></returns>
        public IConverter<F, T> GetConverter<F, T>()
        {
            var ft = new FT(typeof(F), typeof(T));
            if (_converterDic.TryGetValue(ft, out var converter)) {
                return (IConverter<F, T>)converter;
            }
            Debug.LogError($"请实现从{typeof(F).Name}转变为{typeof(T).Name}的转换器,继承AConverter<F,T>");
            return null;
        }
        
        public object GetConverter(Type from, Type to)
        {
            var ft = new FT(from, to);
            if (_converterDic.TryGetValue(ft, out var converter)) {
                return converter;
            }
            Debug.LogError($"请实现从{from.Name}转变为{to.Name}的转换器,继承AConverter<F,T>");
            return null;
        }

        public void Dispose()
        {
            _converterDic.Clear();
            _converterDic = null;
            ILHelper.Instance = null;
        }
    }
}
