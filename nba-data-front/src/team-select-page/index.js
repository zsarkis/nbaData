import './team-select-page.css';
import Header from './header';
import { useEffect, useState, useMemo } from "react";
import {BrowserRouter as Router, Switch, Route} from "react-router-dom"
import Team from './team-finding';
import axios from 'axios';

function App() {
  //read useEffect docs
  //call hooks at the top level (not conditional or in loops)
  const [allTeams, setAllTeams] = useState([]);

  useEffect(() => {
    const fetchTeams = async () => {
      await axios.get("https://www.balldontlie.io/api/v1/teams").then((res) => {
        let teamList = [];
        
        for (const team of Object.values(res.data)) {
          teamList.push(team);
        }
        let full_nameTeams = [];

        for (const team of Object.values(teamList[0])) {
          full_nameTeams.push(team.full_name);
        }

        setAllTeams(full_nameTeams);
      });
    };
    fetchTeams();
  }, []);

  return (
    <Router>
      <div className="container">
        <Header subtitle = "Providing shooting metrics by matchup" />
        <Team allTeams={allTeams}/>
      </div>
    </Router>
  );
}

export default App;
