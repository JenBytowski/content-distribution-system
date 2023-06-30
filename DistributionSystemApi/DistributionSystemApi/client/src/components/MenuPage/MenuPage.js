import React from "react";
import './MenuPage.scss';
import Nav from "react-bootstrap/Nav";
import { Container } from "react-bootstrap";

export default function MenuPage() {
  return (
      <Container className="menu">
        <Nav className="nav" id="menu">
          <Nav.Link href="recipient">Recipient List</Nav.Link>
          <Nav.Link href="group">Group List Page</Nav.Link>
          <Nav.Link href="mailTemplates">Mail Templates Page</Nav.Link>
          <Nav.Link href="distributions">Distributions Page</Nav.Link>
        </Nav>
      </Container>
  );
}