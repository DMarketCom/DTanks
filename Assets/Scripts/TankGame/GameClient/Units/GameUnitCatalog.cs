using System.Collections.Generic;

namespace Game
{
    public class GameUnitCatalog
    {
        private int _freeUnitId = 0;

        private readonly Dictionary<int, GameUnitBase> _unitsById 
            = new Dictionary<int, GameUnitBase>();
        private readonly Dictionary<int, GameUnitType> _unitsTypesById
            = new Dictionary<int, GameUnitType>();

        public void AddAs(GameUnitBase unit, GameUnitType unitType)
        {
            var unitId = _freeUnitId;
            _freeUnitId++;
            unit.Initialize(unitId);
            _unitsTypesById.Add(unitId, unitType);
            _unitsById.Add(unitId, unit);
        }

        public GameUnitBase Get(int id)
        {
            return _unitsById.ContainsKey(id) ? _unitsById[id] : null;
        }

        public GameUnitType GetUnitType(int id)
        {
            return _unitsTypesById[id];
        }
    }
}
