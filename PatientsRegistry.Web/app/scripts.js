'use strict';

const e = React.createElement;
const url = 'http://localhost:5000/api/patients';

class Search extends React.Component {
  constructor(props) {
    super(props);
    this.state = { patients: [], search: { name: '', birthdate: '', phone: '' }, error: null };
    this.searchHandler = this.searchHandler.bind(this);
    this.handleChange = this.handleChange.bind(this);
    this.seedHandler = this.seedHandler.bind(this);
  }

  handleChange(e) {
    const target = event.target;
    var search = Object.assign({}, this.state.search);
    search[target.name] = target.value;
    this.setState({
      search: search
    });
  }

  searchHandler(event) {
    var search = this.state.search;
    console.log(event, search);
    var _this = this;

    $.getJSON(url, search,
    function(json) {
        console.log(json);
        _this.setState({ patients: json });
    })
    .fail(response => {
      console.log(response);
      _this.setState({ error: response.responseText });
    });
  }

  deleteHandler(id, idx){
    console.log(id);
    var _this = this;
    $.ajax({
      type: "DELETE",
      url: url + '/' + id,
      success: function(){
        var patients = [..._this.state.patients];
          
          if (idx !== -1) {
            patients.splice(idx, 1);
            _this.setState({patients: patients});
          }
      }
    });
  }

  seedHandler(){
    $.ajax({
      type: "POST",
      url: ' http://localhost:5000/seed'
    });
  }

  render() {
    return (
      <div >
        <div id="error_reponse">
          <div hidden={this.state.error == null} class="alert alert-danger alert-dismissible fade show" role="alert">
            {this.state.error}
            <button type="button" class="close" aria-label="Close" onClick={() => this.setState({ error: null })}>
              <span aria-hidden="true">&times;</span>
            </button>
          </div>
        </div>
        <form class="card p-2">
          <div class="input-group">
            <input type="text" class="form-control" name="name" value={this.state.search.name} onChange={this.handleChange} placeholder="Name..." />
            <div class="input-group-append">
              <button type="button" class="btn btn-secondary" onClick={this.searchHandler}>Find</button>
            </div>
          </div>
          <div class="row">
            <div class="col-md-5 order-md-1 mb-4 mt-2">
              <div class="input-group">
                <div class="input-group mb-2 mr-sm-2">
                  <div class="input-group-prepend">
                    <div class="input-group-text">Phone</div>
                  </div>
                  <input type="text" class="form-control" name="phone" value={this.state.search.phone} onChange={this.handleChange} />
                </div>
              </div>
            </div>
            <div class="col-md-5 order-md-2 mt-2">
              <div class="input-group">
                  <div class="input-group mb-2 mr-sm-2">
                    <div class="input-group-prepend">
                      <div class="input-group-text">Birthdate</div>
                    </div>
                    <input type="text" class="form-control" name="birthdate" value={this.state.search.birthdate} onChange={this.handleChange} />
                  </div>
                </div>
            </div>
            <div class="col-md-2 order-md-3">
              <button type="button" class="btn btn-outline-success mt-2" onClick={this.seedHandler}>Seed</button>
            </div>
          </div>
        </form>
        <hr class="mb-4" />
        <h4 class="d-flex justify-content-between align-items-center mb-3">
          <span class="text-muted">Search results</span>
          <span class="badge badge-secondary badge-pill">{this.state.patients.length}</span>
        </h4>
        <ul class="list-group mb-3">
          {this.state.patients.map((patient, idx) => (
            <li class="list-group-item d-flex justify-content-between lh-condensed">
              <div>
                <h6 class="my-0">{patient.lastname} {patient.name} {patient.patronymic}</h6>
                <hr class="mb-2" />
                {patient.contacts.map((contact, idx) => (
                  <div class="text-muted">{contact.value} ({contact.kind} {contact.type})</div>
                ))}
              </div>
              <span class="text-muted">{patient.birthdate.substring(0, 10)}</span>
              <div class="text-muted">{patient.gender}</div>
              <button type="button" class="btn btn-danger" onClick={() => this.deleteHandler(patient.id, idx)}>Delete</button>
            </li>
            ))}
          
        </ul>
      </div>
    );
  }
}

class Registration extends React.Component {
  constructor(props) {
    super(props);
    this.state = {
      patient: {
        name: '',
        lastname: '',
        patronymic: '',
        birthdate: '',
        gender: '',
        phone: '',
        contacts: []
      },
      error: null
    };
    this.handleChange = this.handleChange.bind(this);
    this.saveHandler = this.saveHandler.bind(this);
  }

  handleChange(event) {
    const target = event.target;
    var patient = Object.assign({}, this.state.patient);
    patient[target.name] = target.value;
    this.setState({
      patient: patient
    });
  }

  saveHandler(event){
    var patient = Object.assign({}, this.state.patient);
    patient.phone = '+380' + patient.phone;
    console.log(patient);
    var _this = this;
    $.ajax({
      type: "POST",
      url: url,
      data: JSON.stringify(patient),
      contentType: "application/json; charset=utf-8",
      dataType: "json",
      success: function(data){
        console.log(data);
        _this.setState({patient: {
          name: '',
          lastname: '',
          patronymic: '',
          birthdate: '',
          gender: '',
          phone: '',
          contacts: []
        }});
        alert('OK');
      },
      error: function(errMsg) {
          console.log(errMsg);
          _this.setState({ error: errMsg.responseText });
      }
    });

  }
  
  render() {
    return (
      <div >
        <div id="error_reponse">
          <div hidden={this.state.error == null} class="alert alert-danger alert-dismissible fade show" role="alert">
            {this.state.error}
            <button type="button" class="close" aria-label="Close" onClick={() => this.setState({ error: null })}>
              <span aria-hidden="true">&times;</span>
            </button>
          </div>
        </div>
        <h4 class="mb-3">Register patient</h4>
          <form class="needs-validation" novalidate>
            <div class="row">
              <div class="col-md-6 mb-3">
                <label for="firstName">First name</label>
                <input type="text" class="form-control" id="firstName" placeholder="" name="name" value={this.state.patient.name} onChange={this.handleChange} required/>
                <div class="invalid-feedback">
                  Valid first name is required.
                </div>
              </div>
              <div class="col-md-6 mb-3">
                <label for="lastName">Last name</label>
                <input type="text" class="form-control" id="lastName" placeholder="" name="lastname" value={this.state.patient.lastname} onChange={this.handleChange} required/>
                <div class="invalid-feedback">
                  Valid last name is required.
                </div>
              </div>
              <div class="col-md-6 mb-3">
                <label for="patronymic">Patronymic</label>
                <input type="text" class="form-control" id="patronymic" placeholder="Optional" name="patronymic" value={this.state.patient.patronymic} onChange={this.handleChange} />
              </div>
              <div class="col-md-6 mb-3">
                <label for="gender">Gender</label>
                <select class="custom-select d-block w-100" id="gender" name="gender" value={this.state.patient.gender} onChange={this.handleChange} required>
                  <option value="" disabled>Choose...</option>
                  <option value="Male">Male</option>
                  <option value="Female">Female</option>
                  <option value="Other">Other</option>
                </select>
                <div class="invalid-feedback">
                  Please select a gender.
                </div>
              </div>
            </div>

            <div class="mb-3">
              <label for="phone_number">Phone</label>
              <div class="input-group">
                <div class="input-group-prepend">
                  <span class="input-group-text">+380</span>
                </div>
                <input type="text" class="form-control" id="phone_number" name="phone" value={this.state.patient.phone} onChange={this.handleChange} required/>
              </div>
            </div>

            <div class="mb-3">
              <label for="birthday">Birthday</label>
              <input type="text" class="form-control" id="birthday" name="birthdate" value={this.state.patient.birthdate} onChange={this.handleChange} required/>
              <div class="invalid-feedback">
                Please enter your birthday.
              </div>
            </div>

            <hr class="mb-4"/>
            <button class="btn btn-primary btn-lg btn-block" type="button" onClick={this.saveHandler}>Register</button>
          </form>
      </div>
    );
  }
}

const domContainer = document.getElementById('react_root');
ReactDOM.render(
  <div class="row">
    <div class="col-md-8 order-md-2 mb-4"><Search /></div>
    <div class="col-md-4 order-md-1"><Registration/></div>
  </div>
, domContainer);