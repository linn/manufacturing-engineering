/**
 * @jest-environment jsdom
 */
import React from 'react';
import '@testing-library/jest-dom/extend-expect';
import { useSelector } from 'react-redux';
import render from '../../test-utils';
import App from '../App';
import actions from '../../actions';

jest.mock('react-redux', () => ({
    ...jest.requireActual('react-redux'),
    useSelector: jest.fn()
}));

const mockAppState = { oidc: { user: { profile: { name: 'User Name' } } } };
const testActionSpy = jest.spyOn(actions, 'testAction');

describe('App tests', () => {
    beforeEach(() => {
        useSelector.mockClear();
        useSelector.mockImplementation(callback => callback(mockAppState));
    });
    test('App renders without crashing...', () => {
        const { getByText } = render(<App />);
        expect(getByText('App')).toBeInTheDocument();
    });

    test('App loads data from redux store...', () => {
        const { getByText } = render(<App />);
        expect(getByText('Hello User Name')).toBeInTheDocument();
    });

    test('App dispatches action...', () => {
        expect(testActionSpy).toBeCalled();
    });
});
