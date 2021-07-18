import {useLocation} from 'react-router-dom';
import { useEffect, useState } from "react";
import Button from 'react-bootstrap/Button';
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
  
  const handleClick = (e) => {
    //redirect
  }

  return ( 
    <div className="row mt-3">
    <div className="offset-md-2 col-md-4">
      Select Players:
    </div>
    <div className="col-md-4 mb-3">
      <select className="form-select">
        {rosterA.map((c) => (
          <option key={c.id} value={c.id}>
            {c.first_name} {c.last_name}
          </option>
        ))}
      </select>
      <select className="form-select">
        {rosterA.map((c) => (
          <option key={c.id} value={c.id}>
            {c.first_name} {c.last_name}
          </option>
        ))}
      </select>
      <select className="form-select">
        {rosterA.map((c) => (
          <option key={c.id} value={c.id}>
            {c.first_name} {c.last_name}
          </option>
        ))}
      </select>
      <select className="form-select">
        {rosterA.map((c) => (
          <option key={c.id} value={c.id}>
            {c.first_name} {c.last_name}
          </option>
        ))}
      </select>
      <select className="form-select">
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
    </div><select className="form-select">
        {rosterB.map((c) => (
          <option key={c.id} value={c.id}>
            {c.first_name} {c.last_name}
          </option>
        ))}
      </select>
      <select className="form-select">
        {rosterB.map((c) => (
          <option key={c.id} value={c.id}>
            {c.first_name} {c.last_name}
          </option>
        ))}
      </select>
      <select className="form-select">
        {rosterB.map((c) => (
          <option key={c.id} value={c.id}>
            {c.first_name} {c.last_name}
          </option>
        ))}
      </select>
      <select className="form-select">
        {rosterB.map((c) => (
          <option key={c.id} value={c.id}>
            {c.first_name} {c.last_name}
          </option>
        ))}
      </select>
      <select className="form-select">
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
        Get Shooting Stats
      </Button>
    </div>
  </div>
  );
}

export default ShooterData;
