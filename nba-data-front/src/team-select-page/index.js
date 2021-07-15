import './team-select-page.css';
import Header from './header';
import Versus from './versus';
import { useEffect, useState } from "react";
import {BrowserRouter as Router} from "react-router-dom"
import Team from './team-finding';
import Matchup from './matchup-select';
import axios from 'axios';

function App() {
  //read useEffect docs
  //call hooks at the top level (not conditional or in loops)
  const [allTeams, setAllTeams] = useState([]);
  //TODO: figure out if this is right to cache teams from dropdown
  const [selectedTeams, setSelectedTeams] = useState([]);

  useEffect(() => {
    const fetchTeams = async () => {
      await axios.get("https://www.balldontlie.io/api/v1/teams").then((res) => {
        const teamList = [];
        for (const team of res.data.data) {
          teamList.push(team.full_name);
        }
        
        setAllTeams(teamList);
      });
    };
    fetchTeams();
  }, []);

  return (
    <Router>
      <div className="container">
        <Header subtitle = "Providing shooting metrics by matchup" />
        <Matchup allTeams={allTeams}/>
      </div>
    </Router>
  );
}

export default App;
