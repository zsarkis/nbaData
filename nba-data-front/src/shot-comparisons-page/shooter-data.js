import Button from 'react-bootstrap/Button';
import {useLocation} from 'react-router-dom';
import { useEffect, useState } from "react";
import queryString from 'query-string';
import axios from 'axios';

const ShooterData = () => {
  const location = useLocation();
  const search = location.search;
  let params = queryString.parse(search)
  const teamA = params['teamA'];
  const teamB = params['teamB'];
  const [rosterA, setRosterA] = useState([]);
  const [rosterB, setRosterB] = useState([]);
  const fullRoster = new Array(10);
  const [selectedRoster, setSelectedRoster] = useState([]);

  useEffect(() => {
    const fetchTeams = async () => {
      axios.defaults.headers.get['Access-Control-Allow-Origin'] = '*';
      axios.defaults.headers.get['Content-Type'] = 'application/x-www-form-urlencoded';
      await axios.get(`https://localhost:5001/api/v1/players`, { params: {teamName: teamA}}).then((res) => {
        const roster = [];
        for (const player of res.data) {
          roster.push(player);
        }
        
        setRosterA(roster);
      });
      await axios.get(`https://localhost:5001/api/v1/players`, { params: {teamName: teamB}}).then((res) => {
        const roster = [];
        for (const player of res.data) {
          roster.push(player);
        }
        
        setRosterB(roster);
      });
    };
    fetchTeams();
  }, []);
  
  //add all selections to a list based off of position in array
  //add button to search based off of contents of roster selections
  //this implementation will have a bug if the selected value doesn't change.

  const handleClick = (e) => {
    //redirect
    console.log('acquiring full shooting stats...');  
  }

  const onPlayer0Change = (e) => {
    const playerSelected = e.target.value;
    fullRoster[0] = playerSelected;
  }

  const onPlayer1Change = (e) => {
    const playerSelected = e.target.value;
    fullRoster[1] = playerSelected;
  }

  const onPlayer2Change = (e) => {
    const playerSelected = e.target.value;
    fullRoster[2] = playerSelected;
  }

  const onPlayer3Change = (e) => {
    const playerSelected = e.target.value;
    fullRoster[3] = playerSelected;
  }

  const onPlayer4Change = (e) => {
    const playerSelected = e.target.value;
    fullRoster[4] = playerSelected;
  }

  const onPlayer5Change = (e) => {
    const playerSelected = e.target.value;
    fullRoster[5] = playerSelected;
  }

  const onPlayer6Change = (e) => {
    const playerSelected = e.target.value;
    fullRoster[6] = playerSelected;
  }

  const onPlayer7Change = (e) => {
    const playerSelected = e.target.value;
    fullRoster[7] = playerSelected;
  }

  const onPlayer8Change = (e) => {
    const playerSelected = e.target.value;
    fullRoster[8] = playerSelected;
  }

  const onPlayer9Change = (e) => {
    const playerSelected = e.target.value;
    fullRoster[9] = playerSelected;
  }


  return ( 
    <div className="row mt-3">
    <div className="offset-md-2 col-md-4">
      Select Players:
    </div>
    <div className="col-md-4 mb-3">
      <select className="form-select" onChange={onPlayer0Change}>
        {rosterA.map((c) => (
          <option key={c.id} value={c.id}>
            {c.first_name} {c.last_name}
          </option>
        ))}
      </select>
      <select className="form-select" onChange={onPlayer1Change}>
        {rosterA.map((c) => (
          <option key={c.id} value={c.id}>
            {c.first_name} {c.last_name}
          </option>
        ))}
      </select>
      <select className="form-select" onChange={onPlayer2Change}>
        {rosterA.map((c) => (
          <option key={c.id} value={c.id}>
            {c.first_name} {c.last_name}
          </option>
        ))}
      </select>
      <select className="form-select" onChange={onPlayer3Change}>
        {rosterA.map((c) => (
          <option key={c.id} value={c.id}>
            {c.first_name} {c.last_name}
          </option>
        ))}
      </select>
      <select className="form-select" onChange={onPlayer4Change}>
        {rosterA.map((c) => (
          <option key={c.id} value={c.id}>
            {c.first_name} {c.last_name}
          </option>
        ))}
      </select>
    <div className="text-center">
      <p></p>
      Versus
      <p></p>
    </div>
      <select className="form-select" onChange={onPlayer5Change}>
        {rosterB.map((c) => (
          <option key={c.id} value={c.id}>
            {c.first_name} {c.last_name}
          </option>
        ))}
      </select>
      <select className="form-select" onChange={onPlayer6Change}>
        {rosterB.map((c) => (
          <option key={c.id} value={c.id}>
            {c.first_name} {c.last_name}
          </option>
        ))}
      </select>
      <select className="form-select" onChange={onPlayer7Change}>
        {rosterB.map((c) => (
          <option key={c.id} value={c.id}>
            {c.first_name} {c.last_name}
          </option>
        ))}
      </select>
      <select className="form-select" onChange={onPlayer8Change}>
        {rosterB.map((c) => (
          <option key={c.id} value={c.id}>
            {c.first_name} {c.last_name}
          </option>
        ))}
      </select>
      <select className="form-select" onChange={onPlayer9Change}>
      {rosterB.map((c) => (
        <option key={c.id} value={c.id}>
          {c.first_name} {c.last_name}
        </option>
      ))}
    </select>
      <p></p>
    </div>
    <div className="text-center">
    <Button onClick={handleClick}>
        Get Full Shooting Stats
      </Button>
    </div>
  </div>
  );
}

export default ShooterData;
