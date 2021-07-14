import {useHistory} from 'react-router-dom';

const Team = ({allTeams}) => {
  const history = useHistory();
  const teams = allTeams.data || allTeams.length > 0 ? Array.from(new Set(allTeams.map((h) => h))) : [];
  teams.unshift(null);

  const onSearchChange = (e) => {
    //TODO: create matchups
    const team = e.target.value;
    history.push(`/searchresults/${team}`);
  }

  return ( 
    <div className="row mt-3">
    <div className="offset-md-2 col-md-4">
      Look for your matchups:
    </div>
    <div className="col-md-4 mb-3">
      <select className="form-select" onChange={onSearchChange}>
        {teams.map((c) => (
          <option key={c} value={c}>
            {c}
          </option>
        ))}
      </select>
    </div>
  </div>
  );
}
 
export default Team;
