import {useHistory} from 'react-router-dom';
import { useState } from 'react';
import Button from 'react-bootstrap/Button';

const Matchup = ({allTeams}) => {
  const [selectedTeamA, setSelectedTeamA] = useState([]);
  const [selectedTeamB, setSelectedTeamB] = useState([]);

  const history = useHistory();
  const teams = allTeams.data || allTeams.length > 0 ? Array.from(new Set(allTeams.map((h) => h))) : [];
  teams.unshift(null);

  const onSearchChangeA = (e) => {
    const teamSelected = e.target.value;
    setSelectedTeamA(teamSelected);
  }

  const onSearchChangeB = (e) => {
    const teamSelected = e.target.value;
    setSelectedTeamB(teamSelected);
  }

  const handleClick = (e) => {
    //redirect
    console.log(selectedTeamA + " vs " + selectedTeamB);  
    let path = `/matchup?teamA=${selectedTeamA}&teamB=${selectedTeamB}`; 
    history.push(path);
  }

  return ( 
    <div className="row mt-3">
    <div className="offset-md-2 col-md-4">
      Look for your matchups:
    </div>
    <div className="col-md-4 mb-3">
      <select className="form-select" onChange={onSearchChangeA}>
        {teams.map((c) => (
          <option key={c} value={c}>
            {c}
          </option>
        ))}
      </select>
    <div className="text-center">
      <p></p>
      Versus
      <p></p>
    </div>
      <select className="form-select" onChange={onSearchChangeB}>
        {teams.map((c) => (
          <option key={c} value={c}>
            {c}
          </option>
        ))}
      </select>
      <p></p>
    </div>
    <div className="text-center">
    <Button onClick={handleClick}>
        Get Shooting Stats
      </Button>
    </div>
  </div>
  );
}

export default Matchup;
