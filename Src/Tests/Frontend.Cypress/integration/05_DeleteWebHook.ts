
export{}


const  url = "https://localhost:5015/hookloopback";


describe('DeleteWebhook Tests', () => {

  before(()=>{

    cy.resetdb()

  })

  beforeEach(()=>{
    cy.login("testuser","testuser")

    cy.visit("/Hooks")

    cy.contains("WebHook")

    cy.location('pathname', { timeout: 10000 })
    .should('eq', '/Hooks');
  })
  
  //--------------------------------------
  //--------------------------------------
  
  it('Create Test WebHook', () => {

    cy.addDummyWebHook(url)

  });

  //--------------------------------------
  //--------------------------------------

  it('Delete WebHook', () => {
    

    cy.visit("/Hooks")

    cy.get('#__hookContainer')
    .should('contain', url)

    cy.get('button:contains("Delete")')
    .should('be.enabled')
    .click()

    cy.contains("Delete WebHook")

    cy.get('[id=__ModalContainer]')
    .children()
    .get('button')
    .contains('Delete')
    .click()

    cy.get('#__hookContainer')
    .should('not.contain', url)

  });

});
