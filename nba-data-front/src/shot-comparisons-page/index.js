import './shot-comparisons-page.css';
import Header from './header';
import { useEffect, useState } from "react";
import {BrowserRouter as Router, Switch, Route} from "react-router-dom"
import Visualizations from '../shot-comparisons-page'
import axios from 'axios';

function App() {
  //read useEffect docs
  //call hooks at the top level (not conditional or in loops)
  // const [allTeams, setAllTeams] = useState([]);
  // //TODO: figure out if this is right to cache teams from dropdown
  // const [selectedTeams, setSelectedTeams] = useState([]);

  // useEffect(() => {
  //   const fetchPlayers = async () => {
  //     await axios.get("https://www.balldontlie.io/api/v1/teams").then((res) => {
  //       const teamList = [];
  //       for (const team of res.data.data) {
  //         teamList.push(team.full_name);
  //       }
        
  //       setAllTeams(teamList);
  //     });
  //   };
  //   fetchPlayers();
  // }, []);

  return (
    <Router>
      {/* TODO: figure out how to throw on incorrect path */}
      <Route exact path="/matchup">
      <div className="container">
        <Header subtitle = "Providing shooting metrics by matchup" />
      </div>
      </Route>
    </Router>
  );
}

export default App;
