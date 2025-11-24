import React from 'react';
import { describe, test, expect } from 'vitest';
import '@testing-library/jest-dom';
import render from '../../test-utils';
import App from '../App';

describe('App tests', () => {
    test('App renders without crashing...', () => {
        const { getByText } = render(<App />);
        expect(getByText('Manufacturing Engineering')).toBeInTheDocument();
    });
});
