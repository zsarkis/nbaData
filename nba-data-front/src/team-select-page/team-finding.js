import {useHistory} from 'react-router-dom';
import { Component, useState, createRef } from 'react';

class Team extends Component {
  constructor(props)
  {
    super(props);
    this.myRef = createRef();
    this.state = { allTeams : props.allTeams, selectedTeam: ''};
    this.handleChange = this.handleChange.bind(this)
  }

  handleChange(e) {
    console.log(e.target.value);
    this.setState({ selectedTeam: e.target.value });
  }

  render() {
      return ( 
    <div className="row mt-3">
    <div className="offset-md-2 col-md-4">
      Look for your matchups:
    </div>
    <div className="col-md-4 mb-3">
      <select className="form-select" onChange={this.handleChange} ref={this.myRef}>
        {this.props.allTeams.map((c) => (
          <option key={c} value={c}>
            {c}
          </option>
        ))}
      </select>
    </div>
  </div>
  );
  }
}


// const Team = ({allTeams}) => {
//   const [selectedTeam, setSelectedTeam] = useState([]);

//   const history = useHistory();
//   const teams = allTeams.data || allTeams.length > 0 ? Array.from(new Set(allTeams.map((h) => h))) : [];
//   teams.unshift(null);

//   const onSearchChange = (e) => {
//     //TODO: create matchups
//     const teamSelected = e.target.value;
//     setSelectedTeam(teamSelected);
//   }

//   return ( 
//     <div className="row mt-3">
//     <div className="offset-md-2 col-md-4">
//       Look for your matchups:
//     </div>
//     <div className="col-md-4 mb-3">
//       <select className="form-select" onChange={onSearchChange}>
//         {teams.map((c) => (
//           <option key={c} value={c}>
//             {c}
//           </option>
//         ))}
//       </select>
//     </div>
//   </div>
//   );
// }

export default Team;
