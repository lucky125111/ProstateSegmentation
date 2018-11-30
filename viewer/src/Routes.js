import React from "react";
import { Route, Switch } from "react-router-dom";
import AppliedRoute from "./components/AppliedRoute";

import NotFound from "./containers/NotFound";
import ImageEditor from "./containers/ImageEditor";
import Panel from "./containers/Panel";


export default ({ childProps }) =>
  <Switch>
    <AppliedRoute path="/" exact component={Panel} props={childProps} />
    <AppliedRoute path={`/patient/:patientID`} component={ImageEditor} />
    { /* Finally, catch all unmatched routes */ }
    <Route component={NotFound} />
  </Switch>;