import './App.css';
import { BrowserRouter, Routes, Route } from "react-router-dom";
import DistributionsPage from "./components/DistributionsPage/DistributionsPage";
import GroupPage from "./components/GroupPage/GroupPage";
import MailTemplates from "./components/MailTemplatesPage/MailTemplates";
import RecipientPage from "./components/RecipientPage/RecipientPage";
import MenuPage from "./components/MenuPage/MenuPage";

function App() {
  return (
    <BrowserRouter>
      <Routes>
        <Route path="/" element={<MenuPage />} />  
        <Route path="/recipient" element={<RecipientPage />} />
        <Route path="/group" element={<GroupPage />} />
        <Route path="/mailTemplates" element={<MailTemplates />} />
        <Route path="/distributions" element={<DistributionsPage />} />
        <Route path="*" element={<MenuPage/> } />  
      </Routes>
  </BrowserRouter>
  );
}

export default App;
